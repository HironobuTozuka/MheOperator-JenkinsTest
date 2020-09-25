using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PickRequestDoneConsumer : IKafkaConsumer
    {
        private readonly IPickRequestDoneListenerRegistry _pickRequestDoneListenerRegistry;
        private readonly ILogger<PickRequestDoneConsumer> _logger;
        public string TopicId => "PickRequestDoneTopic";
        public bool DeleteTopicOnConnect => false;

        public PickRequestDoneConsumer(ILoggerFactory loggerFactory,
            IPickRequestDoneListenerRegistry pickRequestDoneListenerRegistry)
        {
            _logger = loggerFactory.CreateLogger<PickRequestDoneConsumer>();
            _logger.LogInformation("PickRequestDoneConsumer created");
            _pickRequestDoneListenerRegistry = pickRequestDoneListenerRegistry;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            PickRequestDoneModel pickRequestDone =
                JsonConvert.DeserializeObject<PickRequestDoneModel>(message, new StringEnumConverter());

            await _pickRequestDoneListenerRegistry.NotifyListeners(pickRequestDone);
        }
    }
}