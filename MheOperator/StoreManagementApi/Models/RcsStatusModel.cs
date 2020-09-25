using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models
{
    public class RcsStatusModel
    {
        [JsonProperty("plc_ready")] public bool PlcReady { get; }

        public RcsStatusModel(bool plcReady)
        {
            PlcReady = plcReady;
        }
    }
}