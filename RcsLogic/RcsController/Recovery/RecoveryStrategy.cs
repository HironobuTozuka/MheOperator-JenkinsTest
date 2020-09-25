using System.Collections.Generic;
using RcsLogic.Models;

namespace RcsLogic.RcsController.Recovery
{
    public interface IRecoveryStrategy
    {
        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result);
    }
}