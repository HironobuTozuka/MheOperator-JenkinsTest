using Common.Models.Task;

namespace Common.Models.Transfer
{
    public interface ISortCode
    {
        public string Name { get; }
        public FailReason? FailReason { get; }
    }
}