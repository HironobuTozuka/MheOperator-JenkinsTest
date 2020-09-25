using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Common.Models.Plc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PlcInformationResponseConsumer : IKafkaConsumer
    {
        private readonly IPlcInformationResponseListener _plcInformationResponseListener;
        private readonly ILogger<PlcInformationResponseConsumer> _logger;
        public string TopicId => "PlcInformationResponseTopic";
        public bool DeleteTopicOnConnect => true;

        public PlcInformationResponseConsumer(ILoggerFactory loggerFactory,
            IPlcInformationResponseListener plcInformationResponseListener)
        {
            _logger = loggerFactory.CreateLogger<PlcInformationResponseConsumer>();
            _logger.LogInformation("PlcInformationResponseConsumer created");
            _plcInformationResponseListener = plcInformationResponseListener;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            PlcInformationResponse informationResponse =
                JsonConvert.DeserializeObject<PlcInformationResponse>(message, new StringEnumConverter());

            await _plcInformationResponseListener.NotifyListener(informationResponse);
        }
    }
}