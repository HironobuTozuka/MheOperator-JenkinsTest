using System.Threading.Tasks;
using Common.Models;

namespace PlcRequestQueueService
{
    public interface IPickRequestDoneListenerRegistry
    {
        public Task NotifyListeners(PickRequestDoneModel pickRequestDone);
    }
}