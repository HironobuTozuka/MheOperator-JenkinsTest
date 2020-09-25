using RcsLogic.Models.Device;

namespace RcsLogic.Models
{
    public interface IPrepareForPickingDevice : IDevice
    {
        public void ToteReady(PrepareForPicking prepareForPicking);
    }
}