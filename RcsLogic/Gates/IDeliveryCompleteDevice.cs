using Common.Models.Tote;

namespace RcsLogic.Gates
{
    public interface IDeliveryCompleteDevice
    {
        public void CompleteDelivery(Tote tote);
    }
}