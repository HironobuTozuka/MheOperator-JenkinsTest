using System.Threading.Tasks;
using Common.Models;

namespace PlcRequestQueueService
{
    public interface ITransferRequestListener
    {
        public Task NotifyListener(TransferRequestModel transferRequest);
    }
}