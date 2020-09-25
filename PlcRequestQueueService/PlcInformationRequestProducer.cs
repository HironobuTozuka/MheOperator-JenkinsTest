using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public class PlcInformationRequestProducer : KafkaProducer
    {
        public PlcInformationRequestProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PlcInformationRequestProducer>(), configuration, "PlcInformationRequestTopic")
        {
            _logger.LogInformation("PlcInformationRequestProducer created");
        }

        public async Task Produce(PlcInformationRequest plcInformationRequest)
        {
            await base.Produce(plcInformationRequest);
        }

        protected override string ProducerId()
        {
            return "PlcInformationRequest";
        }
    }
}
