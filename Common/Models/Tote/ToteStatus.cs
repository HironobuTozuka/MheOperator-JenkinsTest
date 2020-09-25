using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Tote
{
    public enum ToteStatus
    {
        [EnumMember(Value = "READY")] [JsonProperty("READY")]
        Ready,
        [EnumMember(Value = "NO_READ")] [JsonProperty("NO_READ")]
        NoRead,
        [EnumMember(Value = "ZONE_NOT_ASSIGNED")] [JsonProperty("ZONE_NOT_ASSIGNED")]
        ZoneNotAssigned,
        [EnumMember(Value = "OVERFILL")] [JsonProperty("OVERFILL")]
        Overfill,
        [EnumMember(Value = "LOCATION_UNKNOWN")] [JsonProperty("LOCATION_UNKNOWN")]
        LocationUnknown
    }
}