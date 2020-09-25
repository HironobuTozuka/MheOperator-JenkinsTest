using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Models;
using Common.Models.Plc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public class PlcActionConsumer : IKafkaConsumer
    {
        private readonly IPlcActionListener _plcActionListener;
        private readonly ILogger<PlcActionConsumer> _logger;
        public string TopicId => "ActionTopic";
        public bool DeleteTopicOnConnect => false;

        public PlcActionConsumer(ILoggerFactory loggerFactory,
            IPlcActionListener plcActionListener) 
        {
            _logger = loggerFactory.CreateLogger<PlcActionConsumer>();
            _logger.LogInformation("PlcActionConsumer created");
            _plcActionListener = plcActionListener;
        }

        public async Task HandleConsumedMessage(string message)
        {
            _logger.LogInformation($"Consumed message '{message}'.");
            PlcAction action = JsonConvert.DeserializeObject<PlcAction>(message, new StringEnumConverter());

            await _plcActionListener.NotifyListener(action);
        }
    }
}