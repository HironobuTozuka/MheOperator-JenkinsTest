using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Location
{
    public enum TemperatureRegime
    {
        [EnumMember(Value = "CHILL")] [JsonProperty("CHILL")]
        Chill,

        [EnumMember(Value = "AMBIENT")] [JsonProperty("AMBIENT")]
        Ambient,

        [EnumMember(Value = "ANY")] [JsonProperty("ANY")]
        Any
    }
}