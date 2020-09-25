using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcCommunicationService.SystemPlc.Models;
using PlcRequestQueueService;

namespace PlcCommunicationService.SystemPlc
{
    public class PlcConnection : PlcCommunicationService.PlcConnection, ITransferRequestListener, IPlcActionListener,
        IPlcInformationRequestListener
    {
        private readonly List<IPlcActionExecutor> _plcActionExecutors = new List<IPlcActionExecutor>();
        private readonly PlcInformationResponseProducer _plcInformationResponseProducer;

        public PlcConnection(ILoggerFactory loggerFactory, IConfiguration configuration) : base(loggerFactory,
            loggerFactory.CreateLogger<PlcConnection>())
        {
            OpcClient = new OpcClient(loggerFactory, configuration);
            MoveRequestSender = new MoveRequestSender(loggerFactory, OpcClient);
            var plcNotificationListener = new PlcNotificationListener(loggerFactory, configuration, MoveRequestSender);
            OpcClient.CreateConnections(plcNotificationListener);

            _plcActionExecutors.Add(new GatePlcActionExecutor());
            _plcActionExecutors.Add(new LedPlcActionExecutor());

            var kafkaConsumerGroup = new KafkaConsumerGroup(loggerFactory, configuration, "SystemPlcConnection");
            kafkaConsumerGroup.Subscribe(new TransferRequestConsumer(loggerFactory, this));
            kafkaConsumerGroup.Subscribe(new PlcActionConsumer(loggerFactory, this));
            kafkaConsumerGroup.Subscribe(new PlcInformationRequestConsumer(loggerFactory, this));

            _plcInformationResponseProducer = new PlcInformationResponseProducer(loggerFactory, configuration);
        }


        public async Task NotifyListener(TransferRequestModel transferRequest)
        {
            var request1 = GetToteRequest(transferRequest.toteRequest1);
            var request2 = GetToteRequest(transferRequest.toteRequest2);

            Logger.LogInformation("Transfer request enqueued to be sent sent to PLC in plcTranslator." +
                                  "request1: ToteId: {1}, Source: {2} Dest: {3}, " +
                                  "request2: ToteId: {1}, Source: {2} Dest: {3}",
                request1.SourceToteBarcode, request1.SourceLocationId, request1.DestLocationId,
                request2.SourceToteBarcode, request2.SourceLocationId, request2.DestLocationId);

            await SendMoveRequest(new SystemPlc.InheritedModels.MoveRequest()
            {
                ToteRequest1 = request1,
                ToteRequest2 = request2
            });
        }

        private ToteRequest GetToteRequest(ToteTransferRequestModel transferRequest)
        {
            ToteRequest toteRequest;
            if (transferRequest != null)
            {
                Logger.LogInformation("transferRequest1 is not null.");
                toteRequest = new ToteRequest(transferRequest);
                Logger.LogInformation("transferRequest1 is not null. Created Tote Request");
            }
            else
            {
                toteRequest = new ToteRequest();
            }

            return toteRequest;
        }

        public async Task NotifyListener(PlcAction plcAction)
        {
            foreach (var executor in _plcActionExecutors) await executor.ExecuteAction(SystemOutSignals(), plcAction);
        }

        private OutSignals SystemOutSignals()
        {
            return (OutSignals) OpcClient.OutSignals;
        }

        public async Task NotifyListener(PlcInformationRequest plcInformationRequest)
        {
            var inSignals = ((InSignals) OpcClient.InSignals);
            await GetBoolValueByKey(plcInformationRequest.Key, inSignals.CnvStatus);

            await GetIntValueByKey(plcInformationRequest.Key, inSignals.Status);
            await GetBoolValueByKey(plcInformationRequest.Key, inSignals.Status);
        }

        private async Task GetBoolValueByKey(string name, object sourceObject)
        {
            var boolValue = await GetBoolValueByMethodName<bool?>(name, sourceObject);
            if (boolValue != null)
            {
                await _plcInformationResponseProducer.Produce(new PlcInformationResponse()
                {
                    Key = name,
                    BoolValue = (bool) boolValue
                });
            }
        }

        private async Task GetIntValueByKey(string name, object sourceObject)
        {
            var cnvLocationStatus = await GetIntValueByMethodName<int?>(name, sourceObject);
            if (cnvLocationStatus != null)
            {
                await _plcInformationResponseProducer.Produce(new PlcInformationResponse()
                {
                    Key = name,
                    IntValue = (int) cnvLocationStatus
                });
            }
        }

        private static async Task<bool?> GetBoolValueByMethodName<T>(string name, object sourceObject)
        {
            // ReSharper disable once PossibleNullReferenceException
            if(sourceObject.GetType().GetMethod(name)?.ReturnType != typeof(Task<bool>)) return null;
            dynamic awaitable = sourceObject.GetType().GetMethod(name)?.Invoke(sourceObject, null);
            if (awaitable != null)
            {
                await awaitable;
                return awaitable.GetAwaiter().GetResult();
            }

            return null;
        }

        private static async Task<int?> GetIntValueByMethodName<T>(string name, object sourceObject)
        {
            // ReSharper disable once PossibleNullReferenceException
            if(sourceObject.GetType().GetMethod(name)?.ReturnType != typeof(Task<int>)) return null;
            dynamic awaitable = sourceObject.GetType().GetMethod(name)?.Invoke(sourceObject, null);
            if (awaitable != null)
            {
                await awaitable;
                return awaitable.GetAwaiter().GetResult();
            }

            return null;
        }
    }
}