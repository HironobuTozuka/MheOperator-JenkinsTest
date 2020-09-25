using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PlcRequestQueueService
{
    public class KafkaConsumerGroup : IDisposable, IKafkaConsumerGroup
    {
        private readonly string _consumerGroupName;
        private readonly ILogger<KafkaConsumerGroup> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly List<IKafkaConsumer> _kafkaConsumers;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly ConsumerConfig _config;


        public KafkaConsumerGroup(ILoggerFactory loggerFactory, IConfiguration configuration, string consumerGroupName = "MainContext")
        {
            _consumerGroupName = consumerGroupName;
            _logger = loggerFactory.CreateLogger<KafkaConsumerGroup>();
            _kafkaConsumers = new List<IKafkaConsumer>();

            var plcRequestQueueServiceConfiguration = configuration.GetSection("PlcRequestQueueService");

            _config = new ConsumerConfig
            {
                GroupId = "mheoperator-consumer-group",
                BootstrapServers = plcRequestQueueServiceConfiguration.GetValue<string>("kafkaHostAddress"),
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            Task.Run(() => StartConsuming(_consumer));
            _logger.LogInformation($"Consumer group {_consumerGroupName} created");
        }

        public void Subscribe(IKafkaConsumer kafkaConsumer)
        {
            _logger.LogDebug($"Consumer group {_consumerGroupName} Subscribed by {kafkaConsumer}");
            if (kafkaConsumer.DeleteTopicOnConnect)
            {
                IAdminClient kafkaAdminClient = new AdminClientBuilder(_config).Build();
                kafkaAdminClient.DeleteTopicsAsync(new []{kafkaConsumer.TopicId});
                kafkaAdminClient.Dispose();
            }
            _kafkaConsumers.Add(kafkaConsumer);
            _consumer.Subscribe(_kafkaConsumers.Select(consumer => consumer.TopicId));
        }

        private async Task StartConsuming(IConsumer<Ignore, string> consumer)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        _logger.LogTrace($"Consumer group {_consumerGroupName} Waiting for new message...");
                        var cr = consumer.Consume(cts.Token);
                        _logger.LogTrace("Consumer returned result");
                        if (cr?.Value != null)
                        {
                            _logger.LogTrace($"Consumer group {_consumerGroupName} Value is not null: {cr.Value}");
                            await _kafkaConsumers.FirstOrDefault(c => c.TopicId == cr.Topic)?
                                .HandleConsumedMessage(cr.Value);
                        }

                        _logger.LogTrace($"Consumer group {_consumerGroupName} Kafka commit");
                        consumer.Commit(cr);
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consumer group {_consumerGroupName} Error occured: {e.Error.Reason}");
                    }
                    catch (KafkaException e)
                    {
                        _logger.LogError(e, $"Consumer group {_consumerGroupName} Error occured in kafka consumer:");
                        _consumer.Subscribe(_kafkaConsumers.Select(c => c.TopicId));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Consumer group {_consumerGroupName} Error occured in kafka consumer:");
                        if (e is OperationCanceledException)
                        {
                            _logger.LogError(e, $"Consumer group {_consumerGroupName} Throwing error further:");
                            throw;
                        };
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                _logger.LogInformation($"Consumer group {_consumerGroupName} Cancelled consumption");
                consumer.Close();
                consumer.Unsubscribe();
            }

        }

        public void Dispose()
        {
            cts.Cancel();
        }
    }
}