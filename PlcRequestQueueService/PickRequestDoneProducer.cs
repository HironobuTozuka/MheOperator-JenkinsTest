using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace PlcRequestQueueService
{
    public class PickRequestDoneProducer : KafkaProducer
    {
        public PickRequestDoneProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PickRequestDoneProducer>(), configuration, "PickRequestDoneTopic")
        {
            _logger.LogInformation("PickRequestDoneProducer created");
        }

        public async Task Produce(PickRequestDoneModel pickRequestDone)
        {
            await base.Produce(pickRequestDone);
        }

        protected override string ProducerId()
        {
            return "PickRequestDone";
        }
    }
}
