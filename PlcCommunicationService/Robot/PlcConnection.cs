using System.Threading.Tasks;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcRequestQueueService;

namespace PlcCommunicationService.Robot
{
    public class PlcConnection : PlcCommunicationService.PlcConnection, IPickRequestBundleListener
    {
        public PlcConnection(ILoggerFactory loggerFactory, IConfiguration configuration) :
            base(loggerFactory, loggerFactory.CreateLogger<PlcConnection>())
        {
            OpcClient = new OpcClient(loggerFactory, configuration);
            MoveRequestSender = new MoveRequestSender(loggerFactory, OpcClient);
            var plcNotificationListener = new PlcNotificationListener(loggerFactory, configuration, MoveRequestSender);
            OpcClient.CreateConnections(plcNotificationListener);
            var kafkaConsumerGroup = new KafkaConsumerGroup(loggerFactory, configuration, "RobotPlcConnection");
            kafkaConsumerGroup.Subscribe(new PickRequestBundleConsumer(loggerFactory,  this));
        }

        public async Task NotifyListener(RobotPickRequestBundleModel pickRequestBundle)
        {
            Logger.LogInformation(
                "Adding pick request in PlcCommunicationService {0}, for part {1}; Preparation request {2} for part {3}",
                pickRequestBundle.pickRequest?.id, pickRequestBundle.pickRequest?.partName,
                pickRequestBundle.preparationRequest?.id, pickRequestBundle.preparationRequest?.partName);
            ToteRequest request1;
            if (pickRequestBundle.pickRequest != null)
            {
                request1 = new ToteRequest(pickRequestBundle.pickRequest);
            }
            //If there is no transfer request 2 we have to send blank to clear data on PLC side, hence request2 cannot be null
            else
            {
                request1 = new ToteRequest();
            }

            ToteRequest request2;
            if (pickRequestBundle.preparationRequest != null)
            {
                request2 = new ToteRequest(pickRequestBundle.preparationRequest);
            }
            //If there is no transfer request 2 we have to send blank to clear data on PLC side, hence request2 cannot be null
            else
            {
                request2 = new ToteRequest();
            }

            await SendMoveRequest(new Robot.InheritedModels.MoveRequest()
            {
                ToteRequest1 = request1,
                ToteRequest2 = request2
            });
        }
    }
}