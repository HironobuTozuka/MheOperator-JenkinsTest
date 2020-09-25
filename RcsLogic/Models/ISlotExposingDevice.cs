using System.Collections.Generic;
using RcsLogic.Models.Device;

namespace RcsLogic.Models
{
    public interface ISlotExposingDevice : IDevice
    {
        public void Expose(SlotsToExpose slotsToExpose);
        public List<ServicedLocation> ServicedLocations { get; }
    }
}