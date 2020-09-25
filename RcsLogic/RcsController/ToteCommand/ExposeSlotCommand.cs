using Common.Models.Location;
using Common.Models.Tote;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.ToteCommand
{
    public class ExposeSlotCommand : IToteCommand
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly Location _scanLocation;
        private readonly Tote _tote;
        private readonly ToteRotation _toteRotation;

        public ExposeSlotCommand(TaskBundleService taskBundleService, DeviceRegistry deviceRegistry, Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            _taskBundleService = taskBundleService;
            _deviceRegistry = deviceRegistry;
            _scanLocation = scanLocation;
            _tote = tote;
            _toteRotation = toteRotation;
        }

        public void Execute()
        {
            var exposeTaskBundle = _taskBundleService.GetDeliverTask(_tote.toteBarcode);
            var slotsToExpose = new SlotsToExpose(exposeTaskBundle, _scanLocation, _toteRotation, _tote);
            
            var device = _deviceRegistry.ChooseDeviceForSlotExposing(slotsToExpose);
            
            device.Expose(slotsToExpose);
        }
    }
}