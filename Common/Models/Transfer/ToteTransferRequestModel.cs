using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Transfer;

namespace Common.Models
{
    public class ToteTransferRequestModel
    {
        public TransferId Id;
        public string SourceLocationId { get; set; }
        public string ToteBarcode { get; set; }
        public RequestToteType ToteType { get; set; }
        public string DestLocationId { get; set; }
        public float Weight { get; set; }
        public float MaxAcc { get; set; }
    }


}
