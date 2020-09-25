using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Tote;

namespace StoreManagementClient.Models
{
    class ToteDataModel
    {
        public string tote_id { get; set; }
        public double max_acc { get; set; }
        public double weight { get; set; }

        public ToteData ToToteData()
        {
            return new ToteData() {maxAcc = max_acc, toteId = tote_id, weight = weight};
        }
    }
}