using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcRequestQueueService;

namespace PlcCommunicationService.Robot
{
    public class PlcNotificationListener : PlcCommunicationService.PlcNotificationListener
    {
        private readonly PickRequestDoneProducer _pickRequestDoneProducer;

        public PlcNotificationListener(ILoggerFactory loggerFactory, IConfiguration configuration,
            IMoveRequestRedStateChangeListener moveRequestRedStateChangeListener)
            : base(loggerFactory.CreateLogger<PlcNotificationListener>(), moveRequestRedStateChangeListener)
        {
            _pickRequestDoneProducer = new PickRequestDoneProducer(loggerFactory, configuration);
        }

        protected override async Task HandlePlcSpecificNotification(PlcReadNotification plcReadNotification,
            PlcCommunicationService.OpcClient opcClient)
        {
        }

        protected override void MoveRequestDone(IMoveRequestConf moveRequestConf)
        {
            _pickRequestDoneProducer.Produce(moveRequestConf.ToteRequest1Conf.ToRobotPickRequestDoneModel()).Wait();
        }
    }
}