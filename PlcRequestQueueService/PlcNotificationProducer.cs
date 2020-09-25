using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public class PlcNotificationProducer : KafkaProducer
    {
        public PlcNotificationProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PlcNotificationProducer>(), configuration, "PlcNotificationTopic")
        {
            _logger.LogInformation("PlcNotificationProducer created");
        }

        public async Task Produce(PlcNotification plcNotification)
        {
            await base.Produce(plcNotification);
        }

        protected override string ProducerId()
        {
            return "PlcNotification";
        }
    }
}
