using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Location
{
    public enum LocationFunction
    {
        [EnumMember(Value = "STORAGE")] [JsonProperty("STORAGE")]
        Storage,

        [EnumMember(Value = "STAGING")] [JsonProperty("STAGING")]
        Staging,

        [EnumMember(Value = "CONVEYOR")] [JsonProperty("CONVEYOR")]
        Conveyor,

        [EnumMember(Value = "PICK")] [JsonProperty("PICK")]
        Pick,
        
        [EnumMember(Value = "PLACE")] [JsonProperty("PLACE")]
        Place,

        [EnumMember(Value = "LOADING_GATE")] [JsonProperty("LOADING_GATE")]
        LoadingGate,

        [EnumMember(Value = "ORDER_GATE")] [JsonProperty("ORDER_GATE")]
        OrderGate,

        [EnumMember(Value = "CRANE")] [JsonProperty("CRANE")]
        Crane,
        
        [EnumMember(Value = "TECHNICAL")] [JsonProperty("TECHNICAL")]
        Technical
    }
}