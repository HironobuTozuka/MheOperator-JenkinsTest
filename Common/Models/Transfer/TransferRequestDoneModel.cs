using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class TransferRequestDoneModel
    {
        public ToteTransferRequestDoneModel transferRequest1Done { get; set; }
        public ToteTransferRequestDoneModel transferRequest2Done { get; set; }

        public TransferRequestDoneModel(ToteTransferRequestDoneModel transferRequest1Conf, ToteTransferRequestDoneModel transferRequest2Conf)
        {
            this.transferRequest1Done = transferRequest1Conf;
            this.transferRequest2Done = transferRequest2Conf;
        }

        public override string ToString() => $"request 1: {transferRequest1Done}; request 2: {transferRequest2Done}";

    }
}
