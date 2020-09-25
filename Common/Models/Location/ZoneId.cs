using Common.JsonConverters;
using Newtonsoft.Json;

namespace Common.Models.Location
{
    [JsonConverter(typeof(JsonSimpleVoToStringConverter))]
    public class ZoneId
    {
        public string Id { get; }

        public ZoneId(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is ZoneId objToCompare && objToCompare.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}