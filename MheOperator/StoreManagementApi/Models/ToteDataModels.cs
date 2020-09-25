using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Tote;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models
{
    public class ToteDataModel
    {
        private ToteDataModel()
        {
        }

        public ToteDataModel(string toteId)
        {
            this.toteId = toteId;
        }

        public ToteDataModel(string toteId, int slotId)
        {
            this.toteId = toteId;
            this.slotId = slotId;
        }


        [JsonProperty("tote_id")] public string toteId { get; private set; }
        [JsonProperty("slot_id")] public int slotId { get; private set; }

        public PickToteData toToteData()
        {
            return new PickToteData()
            {
                slotId = this.slotId,
                toteId = this.toteId
            };
        }
    }
}