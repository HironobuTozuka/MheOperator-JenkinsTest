using Common.JsonConverters;
using Newtonsoft.Json;

namespace Common.Models.Gate
{
    [JsonConverter(typeof(JsonSimpleVoToStringConverter))]
    public class GateId
    {
        public string Id { get; private set; }

        public GateId(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is GateId objToCompare && objToCompare.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}