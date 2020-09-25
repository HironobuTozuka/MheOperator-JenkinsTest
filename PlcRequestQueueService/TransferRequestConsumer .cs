using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class TransferRequestConsumer : IKafkaConsumer
    {
        private readonly ITransferRequestListener _transferRequestListener;
        private readonly ILogger<TransferRequestConsumer> _logger;
        public string TopicId => "TransferRequestTopic";
        public bool DeleteTopicOnConnect => false;

        public TransferRequestConsumer(ILoggerFactory loggerFactory,
            ITransferRequestListener transferRequestListener) 
        {
            _logger = loggerFactory.CreateLogger<TransferRequestConsumer>();
            _logger.LogInformation("TransferRequestConsumer created");
            _transferRequestListener = transferRequestListener;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            TransferRequestModel transferRequestDone =
                JsonConvert.DeserializeObject<TransferRequestModel>(message, new StringEnumConverter());

            await _transferRequestListener.NotifyListener(transferRequestDone);
        }
    }
}