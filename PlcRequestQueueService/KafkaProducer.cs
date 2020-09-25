using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlcRequestQueueService
{
    public abstract class KafkaProducer
    {

        protected readonly ILogger _logger;
        private readonly IProducer<Null, string> _producer;
        private readonly string _topicId;

        protected KafkaProducer(ILogger logger, IConfiguration configuration, string topicId)
        {
            _logger = logger;
            _topicId = topicId;

            var plcRequestQueueServiceConfiguration = configuration.GetSection("PlcRequestQueueService");
            var producerConfig = new ProducerConfig { BootstrapServers = plcRequestQueueServiceConfiguration.GetValue<string>("kafkaHostAddress"), EnableIdempotence = true};

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        protected async Task Produce(object data)
        {
            try
            {
                var startTime = DateTime.Now;
                var serializedMessage = JsonConvert.SerializeObject(data, new StringEnumConverter());
                var serializationExecutionTime = DateTime.Now - startTime;
                startTime = DateTime.Now;
                var dr = await _producer.ProduceAsync(_topicId, new Message<Null, string> { Value = serializedMessage });
                var kafkaExecutionTime = DateTime.Now - startTime;
                _logger.LogInformation($"Delivered {ProducerId()} '{dr.Value}' to '{dr.TopicPartitionOffset} ; processing time: {kafkaExecutionTime:mm\\:ss\\:fffffff} ; serialization time: {serializationExecutionTime:mm\\:ss\\:fffffff}'");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }

        protected abstract string ProducerId();

    }
}
