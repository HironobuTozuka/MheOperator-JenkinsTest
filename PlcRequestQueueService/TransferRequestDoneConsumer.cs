using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class TransferRequestDoneConsumer : IKafkaConsumer
    {
        private readonly ITransferRequestDoneListenerRegistry _transferRequestDoneListenerRegistry;
        private readonly ILogger<TransferRequestDoneConsumer> _logger;
        public string TopicId => "TransferRequestDoneTopic";
        public bool DeleteTopicOnConnect => false;

        public TransferRequestDoneConsumer(ILoggerFactory loggerFactory,
            ITransferRequestDoneListenerRegistry transferRequestDoneListenerRegistry) 

        {
            _logger = loggerFactory.CreateLogger<TransferRequestDoneConsumer>();
            _logger.LogInformation("TransferRequestDoneConsumer created");
            _transferRequestDoneListenerRegistry = transferRequestDoneListenerRegistry;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            TransferRequestDoneModel transferRequestDone =
                JsonConvert.DeserializeObject<TransferRequestDoneModel>(message, new StringEnumConverter());

            await _transferRequestDoneListenerRegistry.NotifyListeners(transferRequestDone);
        }
    }
}