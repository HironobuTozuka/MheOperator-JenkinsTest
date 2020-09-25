using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;

namespace PlcRequestQueueService
{
    public class TransferRequestProducer : KafkaProducer
    {
        public TransferRequestProducer(ILoggerFactory loggerFactory, IConfiguration configuration) 
            : base (loggerFactory.CreateLogger<TransferRequestProducer>(), configuration, "TransferRequestTopic")
        {
            _logger.LogInformation("TransferRequestProducer created");
        }

        public async Task Produce(TransferRequestModel transferRequest)
        {
            await base.Produce(transferRequest);
        }


        protected override string ProducerId()
        {
            return "TransferRequest";
        }
    }
}
