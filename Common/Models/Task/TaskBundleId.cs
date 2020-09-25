using System.Collections.Generic;

namespace Common.Models.Task
{
    public class TaskBundleId
    {
        public string Id { get; }

        public TaskBundleId(string id)
        {
            Id = id;
        }
        
        public override string ToString()
        {
            return Id;
        }

        protected bool Equals(TaskBundleId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskBundleId) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}