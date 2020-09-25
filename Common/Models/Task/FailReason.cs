using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Models.Task
{
    public enum FailReason
    {
        [EnumMember(Value = "DEST_TOTE_ERROR")] [JsonProperty("DEST_TOTE_ERROR")]
        DestToteError,
        [EnumMember(Value = "SOURCE_TOTE_ERROR")] [JsonProperty("SOURCE_TOTE_ERROR")]
        SourceToteError,
        [EnumMember(Value = "SKU_ERROR")] [JsonProperty("SKU_ERROR")]
        SkuError,
        [EnumMember(Value = "ROBOT_ERROR")] [JsonProperty("ROBOT_ERROR")]
        RobotError,
        [EnumMember(Value = "OTHER_PICK_ERROR")] [JsonProperty("OTHER_PICK_ERROR")]
        OtherPickError
    }
}