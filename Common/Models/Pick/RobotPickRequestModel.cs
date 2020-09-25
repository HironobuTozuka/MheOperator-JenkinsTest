using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Pick;
using Common.Models.Tote;

namespace Common.Models
{
    public class RobotPickRequestModel
    {
        public PickId id;
        public ToteRequestModel source { get; set; }
        public ToteRequestModel dest { get; set; }
        public string partName { get; set; }
        public ushort pickCount { get; set; }
    }

    public class ToteRequestModel
    {
        public string locationId { get; set; }
        public string barcode { get; set; }
        public int slotId { get; set; }
        public ToteType toteType { get; set; }

    }
}
