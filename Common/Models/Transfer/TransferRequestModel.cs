using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Tote;

namespace Common.Models
{
    public class TransferRequestModel
    {
        public TransferRequestModel(ToteTransferRequestModel toteRequest1, ToteTransferRequestModel toteRequest2)
        {
            this.toteRequest1 = toteRequest1;
            this.toteRequest2 = toteRequest2;
            TrimNoReads(toteRequest1);
            TrimNoReads(toteRequest2);
        }

        private static void TrimNoReads(ToteTransferRequestModel toteRequest1)
        {
            if (toteRequest1 != null && toteRequest1.ToteBarcode.Contains(Barcode.NoRead))
            {
                toteRequest1.ToteBarcode = Barcode.NoRead;
            }
        }

        public ToteTransferRequestModel toteRequest1 { get; set; }
        public ToteTransferRequestModel toteRequest2 { get; set; }
    }


}
