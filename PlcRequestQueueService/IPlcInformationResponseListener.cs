using System.Threading.Tasks;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public interface IPlcInformationResponseListener
    {
        public Task NotifyListener(PlcInformationResponse plcInformationResponse);
    }
}