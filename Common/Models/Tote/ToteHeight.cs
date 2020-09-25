using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Tote
{
    public enum ToteHeight
    {
        [EnumMember(Value = "UNKNOWN")] [JsonProperty("UNKNOWN")]
        unknown = 175,

        [EnumMember(Value = "LOW")] [JsonProperty("LOW")]
        low = 120,

        [EnumMember(Value = "HIGH")] [JsonProperty("HIGH")]
        high = 170
    }
}