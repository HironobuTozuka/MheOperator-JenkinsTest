using Common.Models;

namespace Common
{
    public interface ITransferRequestDoneListener
    {
        public void ProcessTransferRequestDone(TransferRequestDoneModel moveRequestDone);
    }
}
