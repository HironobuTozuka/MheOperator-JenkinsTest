using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace PlcRequestQueueService
{
    public class TransferRequestDoneProducer : KafkaProducer
    {
        public TransferRequestDoneProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<TransferRequestDoneProducer>(), configuration, "TransferRequestDoneTopic")
        {
            _logger.LogInformation("TransferRequestDoneProducer created");
        }

        public async Task Produce(TransferRequestDoneModel transferRequestDone)
        {
            await base.Produce(transferRequestDone);
        }

        protected override string ProducerId()
        {
            return $"TransferRequestDone";
        }
    }
}
