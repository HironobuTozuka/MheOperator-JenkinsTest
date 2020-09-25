using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models.Plc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PlcInformationRequestConsumer : IKafkaConsumer
    {
        private readonly IPlcInformationRequestListener _plcInformationRequestListener;
        private readonly ILogger<PlcInformationRequestConsumer> _logger;
        public string TopicId => "PlcInformationRequestTopic";
        public bool DeleteTopicOnConnect => true;

        public PlcInformationRequestConsumer(ILoggerFactory loggerFactory,
            IPlcInformationRequestListener plcInformationRequestListener) 
        {
            _logger = loggerFactory.CreateLogger<PlcInformationRequestConsumer>();
            _logger.LogInformation("PlcInformationRequestConsumer created");
            _plcInformationRequestListener = plcInformationRequestListener;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            PlcInformationRequest informationRequest =
                JsonConvert.DeserializeObject<PlcInformationRequest>(message, new StringEnumConverter());

            await _plcInformationRequestListener.NotifyListener(informationRequest);
        }
    }
}