using System;

namespace Common.Models.Task
{
    public abstract class TaskBase
    {
        protected TaskBase()
        {
            taskStatus = RcsTaskStatus.Idle;
        }

        public TaskId taskId { get; set; }
        public RcsTaskStatus taskStatus { get; set; }
        public bool isInternal { get; set; }
        public DateTime processingStartedDate { get; set; }
        public DateTime lastUpdateDate { get; set; }

        public bool IsFinished()
        {
            return taskStatus == RcsTaskStatus.Complete || taskStatus == RcsTaskStatus.Faulted || taskStatus == RcsTaskStatus.Cancelled;
        }

        protected bool Equals(TaskBase other)
        {
            return taskId.Equals(other.taskId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskBase) obj);
        }

        public override int GetHashCode()
        {
            return (taskId != null ? taskId.GetHashCode() : 0);
        }
    }
}