using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Common.Models.Plc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PlcNotificationConsumer : IKafkaConsumer
    {
        private readonly ILogger<PlcNotificationConsumer> _logger;
        private readonly PlcNotificationListenerRegistry _plcNotificationListenerRegistry;
        public string TopicId => "PlcNotificationTopic";
        public bool DeleteTopicOnConnect => true;

        public PlcNotificationConsumer(ILoggerFactory loggerFactory,
             PlcNotificationListenerRegistry plcNotificationListenerRegistry)
        {
            _plcNotificationListenerRegistry = plcNotificationListenerRegistry;
            _logger = loggerFactory.CreateLogger<PlcNotificationConsumer>();
            _logger.LogInformation("PlcNotificationConsumer created");
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogTrace($"Consumed message '{message}'.");
            PlcNotification plcNotification =
                JsonConvert.DeserializeObject<PlcNotification>(message, new StringEnumConverter());

            await _plcNotificationListenerRegistry.NotifyListeners(plcNotification);
        }
    }
}