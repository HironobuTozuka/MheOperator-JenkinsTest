using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Pick;

namespace Common.Models
{
    public class PickRequestDoneModel
    {
        public PickId requestId { get; set; }
        public string sourceLocationId { get; set; }
        public string sourceToteBarcode { get; set; }
        public string destLocationId { get; set; }
        public string destToteBarcode { get; set; }
        public string partName { get; set; }
        public ushort requestedPickCount { get; set; }
        public ushort actualPickCount { get; set; }
        public ushort sortCode { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(requestId)}: {requestId}, " +
                $"{nameof(sourceLocationId)}: {sourceLocationId}, " +
                $"{nameof(sourceToteBarcode)}: {sourceToteBarcode}, " +
                $"{nameof(destLocationId)}: {destLocationId}, " +
                $"{nameof(destToteBarcode)}: {destToteBarcode}, " +
                $"{nameof(partName)}: {partName}, " +
                $"{nameof(requestedPickCount)}: {requestedPickCount}, " +
                $"{nameof(actualPickCount)}: {actualPickCount}, " +
                $"{nameof(sortCode)}: {sortCode}";
        }
    }


}
