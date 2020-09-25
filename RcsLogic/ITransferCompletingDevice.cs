using System.Collections.Generic;
using Common.Models;
using RcsLogic.Models;

namespace RcsLogic
{
    public interface ITransferCompletingDevice : ITransferDevice
    {
        public bool ShouldHandleTransferDone(ToteTransferRequestDoneModel transferRequestDoneModel);
        public Transfer GetCompletedTransfer(ToteTransferRequestDoneModel transferRequestDoneModel);
    }
}