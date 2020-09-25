using System.Threading.Tasks;

namespace PlcRequestQueueService
{
    public interface IKafkaConsumer
    {
        public string TopicId { get; }
        public bool DeleteTopicOnConnect { get; }
        public Task HandleConsumedMessage(string message);
    }
}