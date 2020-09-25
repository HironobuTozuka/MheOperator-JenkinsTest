using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Transfer;

namespace Common.Models
{
    public class ToteTransferRequestDoneModel
    {
        public TransferId requestId { get; set; }
        public string sourceLocationId { get; set; }
        public string sourceToteBarcode { get; set; }
        public string requestedDestLocationId { get; set; }
        public string actualDestLocationId { get; set; }
        public ushort sortCode { get; set; }

        public override string ToString() => $"TransferId: {requestId}, sourceLocation: {sourceLocationId}, " +
                                             $"sourceToteBarcode: {sourceToteBarcode}, " +
                                             $"requestedDestLocation: {requestedDestLocationId}," +
                                             $" actualDestLocation: {actualDestLocationId}, " +
                                             $"sortCode: {sortCode}";
    }


}
