using Common.Models;
using RcsLogic.Models;
using RcsLogic.RcsController.Recovery;

namespace RcsLogic
{
    public class TransferResult
    {
        public int ResultOrdinal;
        public ToteTransferRequestDoneModel RequestDone { get; set; }
        public Transfer RequestedTransfer { get; set; }
        public SystemSortCode SystemSortCode { get; set; }

        public override string ToString()
        {
            return $"{nameof(RequestDone)}: {RequestDone}, {nameof(SystemSortCode)}: {SystemSortCode}";
        }
    }
}