using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common;
using Common.Models;
using Common.Models.Gate;
using Common.Models.Led;
using Common.Models.Plc;
using PlcCommunicationService.Models;
using PlcRequestQueueService;


namespace PlcCommunicationService
{
    public class PlcService : IPlcService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<PlcService> _logger;
        private readonly IConfiguration _configuration;

        private IPlcConnection _systemPlcConnection;
        private IPlcConnection _robotPlcConnection;

        private readonly PickRequestBundleProducer _pickRequestBundleProducer;
        private readonly TransferRequestProducer _transferRequestProducer;
        private readonly PlcActionProducer _plcActionProducer;
        private readonly PlcInformationCommunication _plcInformationCommunication;


        public PlcService(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<PlcService>();
            _configuration = configuration;
            _pickRequestBundleProducer = new PickRequestBundleProducer(loggerFactory, configuration);
            _transferRequestProducer = new TransferRequestProducer(loggerFactory, configuration);
            _plcActionProducer = new PlcActionProducer(loggerFactory, configuration);
            _plcInformationCommunication = new PlcInformationCommunication(loggerFactory, configuration);

            KafkaConsumerGroup kafkaConsumerGroup = new KafkaConsumerGroup(loggerFactory, configuration, "PlcService");
            kafkaConsumerGroup.Subscribe(_plcInformationCommunication.PlcInformationResponseConsumer);
            
            if (string.IsNullOrEmpty(configuration[$"PlcTranslatorSettings:connectToPlc"]) 
                || bool.Parse(configuration[$"PlcTranslatorSettings:connectToPlc"]))
            {
                Connect();
            }
        }

        private void Connect()
        {
            //Create opc clients
            if (string.IsNullOrEmpty(_configuration[$"PlcTranslatorSettings:connectToRobotPlc"]) 
                || bool.Parse(_configuration[$"PlcTranslatorSettings:connectToRobotPlc"]))
            {
                _robotPlcConnection = new Robot.PlcConnection(_loggerFactory, _configuration);
            }
            if (string.IsNullOrEmpty(_configuration[$"PlcTranslatorSettings:connectToSystemPlc"]) 
                || bool.Parse(_configuration[$"PlcTranslatorSettings:connectToSystemPlc"]))
            {
                _systemPlcConnection = new SystemPlc.PlcConnection(_loggerFactory, _configuration);
            }

        }

        public void SwitchLed(LedId ledId, LedState ledState)
        {
            _logger.LogInformation("Turning led {0} to on: {1}", ledId, ledState);

            _plcActionProducer.Produce(new PlcAction()
            {
                ActionType = PlcActionType.Led,
                Parameters = new PlcActionParameters()
                {
                    LedId = ledId,
                    LedState = ledState
                }
            });
        }

        public void OpenGate(GateDescription gate)
        {
            _logger.LogInformation("Opening gate {0}", gate.gateId);
            Gate(gate, GateAction.Open);
        }

        public void CloseGate(GateDescription gate)
        {
            _logger.LogInformation("Closing gate {0}", gate.gateId);
            Gate(gate, GateAction.Close);
        }

        private void Gate(GateDescription gate, GateAction gateAction)
        {
            _plcActionProducer.Produce(new PlcAction()
            {
                ActionType = PlcActionType.Gate,
                Parameters = new PlcActionParameters()
                {
                    GateDescription = gate,
                    GateAction = gateAction
                }
            });
        }

        public bool IsConveyorOccupied(string locationId)
        {
            _logger.LogInformation("Retrieving location {0} occupancy", locationId);
            if (locationId == "CNV1")
            {
                return ((_plcInformationCommunication.RetrieveInformation(new PlcInformationRequest()
                           {Key = $"CNV1_2Occupied"}))?.BoolValue ?? false)
                       || (_plcInformationCommunication
                               .RetrieveInformation(new PlcInformationRequest() {Key = $"CNV1_1Occupied"})?.BoolValue ??
                           true);
            }
            else
            {
                return _plcInformationCommunication
                    .RetrieveInformation(new PlcInformationRequest() {Key = $"{locationId}Occupied"})?
                    .BoolValue ?? false;
            }
        }

        public bool IsPlcInExecute()
        {
            return _plcInformationCommunication
                       .RetrieveInformation(new PlcInformationRequest() {Key = $"PackMLState"})?.IntValue == 6
                   && _plcInformationCommunication
                       .RetrieveInformation(new PlcInformationRequest() {Key = $"PackMLMode"})?.IntValue == 1;
        }
        
        public void RequestPick(RobotPickRequestBundleModel requestBundle)
        {
            _pickRequestBundleProducer.Produce(requestBundle);
        }

        public void RequestTransfer(TransferRequestModel transferRequest)
        {
            if (transferRequest.toteRequest1 == null && transferRequest.toteRequest2 == null)
            {
                _logger.LogError("Enqueued empty transfer request!!! Aborting on sending!!!");
                return;
            }

            _transferRequestProducer.Produce(transferRequest);
        }
    }
}