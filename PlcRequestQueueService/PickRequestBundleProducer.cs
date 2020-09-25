using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace PlcRequestQueueService
{
    public class PickRequestBundleProducer : KafkaProducer
    {
        public PickRequestBundleProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PickRequestBundleProducer>(), configuration, "PickRequestBundleTopic")
        {
            _logger.LogInformation("PickRequestBundleProducer created");
        }

        public async Task Produce(RobotPickRequestBundleModel pickRequestBundle)
        {
            await base.Produce(pickRequestBundle);
        }

        protected override string ProducerId()
        {
            return "PickRequestBundle";
        }
    }
}
