using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PickRequestBundleConsumer : IKafkaConsumer
    {
        private readonly IPickRequestBundleListener _pickRequestBundleListener;
        private readonly ILogger<PickRequestBundleConsumer> _logger;
        public string TopicId => "PickRequestBundleTopic";
        public bool DeleteTopicOnConnect => false;

        public PickRequestBundleConsumer(ILoggerFactory loggerFactory,
            IPickRequestBundleListener pickRequestBundleListener)
        {
            _logger = loggerFactory.CreateLogger<PickRequestBundleConsumer>();
            _logger.LogInformation("PickRequestBundleConsumer created");
            _pickRequestBundleListener = pickRequestBundleListener;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            RobotPickRequestBundleModel pickRequestBundle =
                JsonConvert.DeserializeObject<RobotPickRequestBundleModel>(message, new StringEnumConverter());

            await _pickRequestBundleListener.NotifyListener(pickRequestBundle);
        }
    }
}