using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public interface IPlcActionListener
    {
        public Task NotifyListener(PlcAction plcAction);
    }
}