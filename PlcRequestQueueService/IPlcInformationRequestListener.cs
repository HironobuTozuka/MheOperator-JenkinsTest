using System.Threading.Tasks;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public interface IPlcInformationRequestListener
    {
        public Task NotifyListener(PlcInformationRequest plcInformationRequest);
    }
}