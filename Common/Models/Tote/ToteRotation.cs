using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Tote
{
    public enum ToteRotation
    {
        [EnumMember(Value = "UNKNOWN")] [JsonProperty("UNKNOWN")]
        unknown = 0,

        [EnumMember(Value = "NORMAL")] [JsonProperty("NORMAL")]
        normal = 1,

        [EnumMember(Value = "REVERSED")] [JsonProperty("REVERSED")]
        reversed = 2
    }

}