using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Tote
{
    public enum TotePartitioning
    {
        [EnumMember(Value = "UNKNOWN")] [JsonProperty("UNKNOWN")]
        unknown,

        [EnumMember(Value = "BIPARTITE")] [JsonProperty("BIPARTITE")]
        bipartite,

        [EnumMember(Value = "TRIPARTITE")] [JsonProperty("TRIPARTITE")]
        tripartite
    }
}