using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace PlcRequestQueueService
{
    public class ScanNotificationProducer : KafkaProducer
    {
        public ScanNotificationProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<ScanNotificationProducer>(), configuration, "ScanNotificationTopic")
        {
            _logger.LogInformation("ScanNotificationProducer created");
        }

        public async Task Produce(ScanNotificationModel scanNotification)
        {
            await base.Produce(scanNotification);
        }

        protected override string ProducerId()
        {
            return $"ScanNotification";
        }
    }
}
