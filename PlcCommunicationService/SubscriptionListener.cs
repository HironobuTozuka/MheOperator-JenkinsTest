using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcRequestQueueService;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService
{
    public class SubscriptionListener
    {
        private readonly ILogger<SubscriptionListener> _logger;
        private readonly List<SubscribedSignal> _subscribedSignals;
        private readonly IPlcReadNotificationListener _notificationProducer;
        private readonly string _clientId;
        private readonly OpcClient _opcClient;
        private Task _subscriptionTask;


        public SubscriptionListener(ILoggerFactory loggerFactory, string clientId,
            List<MonitoredItemDefinition> monitoredItems, OpcClient opcClient,
            IPlcReadNotificationListener notificationProducer)
        {
            _logger = loggerFactory.CreateLogger<SubscriptionListener>();
            _subscribedSignals = monitoredItems.Select(item => new SubscribedSignal() {MonitoredItem = item}).ToList();
            _clientId = clientId;
            _notificationProducer = notificationProducer;
            _opcClient = opcClient;
            Subscribe();
        }

        private void Subscribe()
        {
            _subscriptionTask = Task.Run(HandleSubscription);
        }


        private async Task HandleSubscription()
        {
            while (true)
            {
                if (_opcClient.AllCommunicationOK)
                {
                    try
                    {
                        ReadCurrentState();
                        SendNotificationsToListener();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"{_clientId} Error thrown in OPC UA subscription: ");
                        _opcClient.ChannelFaulted();
                        if ((ex as OutOfMemoryException) != null) throw;
                    }
                }

                await Task.Delay(100);
            }
        }

        private void ReadCurrentState()
        {
            var nodesToRead = _subscribedSignals.Select(item => item.MonitoredItem.nodeId).ToArray();
            var readTask = _opcClient.SignalReader.ReadVariables(nodesToRead);
            readTask.Wait();
            for (var i = 0; i < readTask.Result.Length; i++)
            {
                var subscribedSignal =
                    _subscribedSignals.First(signal => signal.MonitoredItem.nodeId == nodesToRead[i]);
                subscribedSignal.Value = readTask.Result[i].GetValueOrDefault<bool>();
            }
        }

        private void SendNotificationsToListener()
        {
            foreach (var subscribedSignal in _subscribedSignals.Where(signal => signal.ValueChanged))
            {
                try
                {
                    _notificationProducer.NotifyListener(new PlcReadNotification()
                    {
                        Key = subscribedSignal.MonitoredItem.propertyName,
                        Value = subscribedSignal.Value
                    }, _opcClient);
                    ;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{_clientId} Error thrown in OPC UA subscription while sending to kafka: ");
                }
            }
        }
    }
}