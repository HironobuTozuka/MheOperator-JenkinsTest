using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public class PlcInformationResponseProducer : KafkaProducer
    {
        public PlcInformationResponseProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PlcInformationResponseProducer>(), configuration, "PlcInformationResponseTopic")
        {
            _logger.LogInformation("PlcInformationResponseProducer created");
        }

        public async Task Produce(PlcInformationResponse plcInformationResponse)
        {
            await base.Produce(plcInformationResponse);
        }

        protected override string ProducerId()
        {
            return "PlcInformationResponse";
        }
    }
}
