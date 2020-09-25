using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public class PlcActionProducer : KafkaProducer
    {
        public PlcActionProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<PlcActionProducer>(), configuration, "ActionTopic")
        {
            _logger.LogInformation("PlcActionProducer created");
        }

        public async Task Produce(PlcAction plcAction)
        {
            await base.Produce(plcAction);
        }

        protected override string ProducerId()
        {
            return "Action";
        }
    }
}
