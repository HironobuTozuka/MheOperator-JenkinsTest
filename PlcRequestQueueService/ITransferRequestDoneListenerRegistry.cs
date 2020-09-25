using System.Threading.Tasks;
using Common.Models;

namespace PlcRequestQueueService
{
    public interface ITransferRequestDoneListenerRegistry
    {
        public Task NotifyListeners(TransferRequestDoneModel transferRequestDone);
    }
}