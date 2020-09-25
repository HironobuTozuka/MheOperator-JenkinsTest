using Common.Models;
using Common.Models.Tote;
using Newtonsoft.Json;

namespace StoreManagementClient.Models
{
    class ToteNotificationModel
    {
        [JsonProperty("tote_id")] public string toteId { get; set; }
        [JsonProperty("tote_partitioning")] public TotePartitioning totePartitioning { get; set; }
        [JsonProperty("tote_height")] public ToteHeight toteHeight { get; set; }
        [JsonProperty("tote_orientation")] public ToteRotation toteRotation { get; set; }
        [JsonProperty("tote_status")] public ToteStatus toteStatus { get; set; }
        [JsonProperty("location")] public string location { get; set; }
    }
}