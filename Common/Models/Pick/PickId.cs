using System;
using Common.JsonConverters;
using Common.Models.Task;
using Newtonsoft.Json;

namespace Common.Models.Pick
{
    [JsonConverter(typeof(JsonSimpleVoToStringConverter))]
    public class PickId
    {
        public string Id { get; }

        public PickId(string id)
        {
            Id = id;
        }
        
        public PickId(TaskId id)
        {
            Id = id.Id;
        }

        public override string ToString()
        {
            return Id;
        }

        public string GetPlcString()
        {
            return Id.Replace("-","");
        }

        public TaskId GetTaskId()
        {
            return Guid.TryParse(Id, out var parsed) ? new TaskId(parsed.ToString()) : new TaskId(Id);
        }

        protected bool Equals(PickId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PickId) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}