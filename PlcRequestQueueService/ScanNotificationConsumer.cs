using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class ScanNotificationConsumer : IKafkaConsumer
    {
        private readonly IScanNotificationListenerRegistry _scanNotificationListenerRegistry;
        private readonly ILogger<ScanNotificationConsumer> _logger;
        public string TopicId => "ScanNotificationTopic";
        public bool DeleteTopicOnConnect => false;

        public ScanNotificationConsumer(ILoggerFactory loggerFactory,
            IScanNotificationListenerRegistry scanNotificationListenerRegistry) 
        {
            _logger = loggerFactory.CreateLogger<ScanNotificationConsumer>();
            _logger.LogInformation("ScanNotificationConsumer created");
            _scanNotificationListenerRegistry = scanNotificationListenerRegistry;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            ScanNotificationModel scanNotification =
                JsonConvert.DeserializeObject<ScanNotificationModel>(message, new StringEnumConverter());

            await _scanNotificationListenerRegistry.NotifyListeners(scanNotification);
        }
    }
}