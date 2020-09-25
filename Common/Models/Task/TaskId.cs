using System.Collections.Generic;

namespace Common.Models.Task
{
    public class TaskId
    {
        public string Id { get; }

        public TaskId(string id)
        {
            Id = id;
        }
        
        public override string ToString()
        {
            return Id;
        }

        protected bool Equals(TaskId other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskId) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}