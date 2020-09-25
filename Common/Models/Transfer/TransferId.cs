using System;
using Common.JsonConverters;
using Common.Models.Task;
using Newtonsoft.Json;

namespace Common.Models.Transfer
{
    [JsonConverter(typeof(JsonSimpleVoToStringConverter))]
    public class TransferId
    {
        public string Id { get; }

        public TransferId(string id)
        {
            Id = id;
        }
        
        public TransferId(TaskId id)
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

        protected bool Equals(TransferId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransferId) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}