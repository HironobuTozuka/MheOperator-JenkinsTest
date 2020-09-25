namespace Common.Models.Task
{
    public enum RcsTaskStatus
    {
        Idle,
        Executing,
        Picking,
        Complete,
        Faulted,
        Cancelled
    };
}