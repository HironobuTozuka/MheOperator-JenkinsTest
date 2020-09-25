namespace PlcRequestQueueService
{
    public interface IKafkaConsumerGroup
    {
        public void Subscribe(IKafkaConsumer kafkaConsumer);
    }
}