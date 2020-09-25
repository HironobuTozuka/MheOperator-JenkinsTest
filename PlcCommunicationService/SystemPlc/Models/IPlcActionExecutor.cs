using System.Threading.Tasks;
using Common.Models.Plc;

namespace PlcCommunicationService.SystemPlc.Models
{
    public interface IPlcActionExecutor
    {
        public Task ExecuteAction(OutSignals outSignals, PlcAction plcAction);
    }
}