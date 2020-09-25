using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.Services;

namespace RcsLogic.RcsController
{
    public class DeviceRegistry
    {
        private readonly Dictionary<ITransferDevice, List<ServicedLocation>> _transferDevices =
            new Dictionary<ITransferDevice, List<ServicedLocation>>();
        
        private readonly Dictionary<IPrepareForPickingDevice, List<ServicedLocation>> _pickingDevices =
            new Dictionary<IPrepareForPickingDevice, List<ServicedLocation>>();
        
        private readonly Dictionary<ISlotExposingDevice, List<ServicedLocation>> _slotExposingDevices =
            new Dictionary<ISlotExposingDevice, List<ServicedLocation>>();

        private readonly List<ITransferCompletingDevice> _transferCompletingDevices =
            new List<ITransferCompletingDevice>();

        private readonly List<IDevice> _devices;

        private readonly RoutingService _routingService;
        private readonly ILogger<DeviceRegistry> _logger;

        public DeviceRegistry(ServicedLocationProvider servicedLocationProvider,
            RoutingService routingService,
            IDeviceInitializer deviceInitializer, ILoggerFactory loggerFactory)
        {
            _devices = deviceInitializer.GetDevices();

            _routingService = routingService;
            _logger = loggerFactory.CreateLogger<DeviceRegistry>();

            GetDevicesOfType<ITransferDevice>().ForEach(it => _transferDevices[it] = servicedLocationProvider.GetServicedLocations(it.DeviceId));

            GetDevicesOfType<IPrepareForPickingDevice>().ForEach(it => _pickingDevices[it] = servicedLocationProvider.GetRobotServicedLocations());

            GetDevicesOfType<ISlotExposingDevice>().ForEach(it => _slotExposingDevices[it] = it.ServicedLocations);
            
            GetDevicesOfType<ITransferCompletingDevice>().ForEach(_transferCompletingDevices.Add);
        }
        
        public ITransferDevice ChooseDeviceForTransfer(Transfer transfer)
        {
            var deviceExists = _transferDevices
                .Where(it => ServicedLocationsContains(it.Value, transfer))
                .Any(it => _routingService.IsRoutedBy(it.Key.DeviceId, transfer.sourceLocation, transfer.destLocation));

            if (!deviceExists) throw new NoDeviceCanHandleTransferException() {Transfer = transfer};

            var potentialDevice = _transferDevices
                .Where(it => ServicedLocationsContains(it.Value, transfer))
                .First(it =>
                    _routingService.IsRoutedBy(it.Key.DeviceId, transfer.sourceLocation, transfer.destLocation));


            return potentialDevice.Key;
        }

        public List<T> GetDevicesOfType<T>()
        {
            return _devices.OfType<T>().ToList();
        }
        
        public IPrepareForPickingDevice ChooseDeviceForPickingPreparation(PrepareForPicking prepareForPicking)
        {
            var deviceExists = _pickingDevices 
                .Any(it => ServicedLocationsContains(it.Value, prepareForPicking));

            if (!deviceExists) throw new NoDeviceCanHandlePickingPreparationException() {PrepareForPicking = prepareForPicking};

            var potentialDevice = _pickingDevices 
                .First(it => ServicedLocationsContains(it.Value, prepareForPicking));

            return potentialDevice.Key;
        }
        
        public ISlotExposingDevice ChooseDeviceForSlotExposing(SlotsToExpose slotsToExpose)
        {
            var deviceExists = _slotExposingDevices 
                .Any(it => ServicedLocationsContains(it.Value, slotsToExpose));

            if (!deviceExists) throw new NoDeviceCanHandleSlotExposingException() {SlotsToExpose = slotsToExpose};

            var potentialDevice = _slotExposingDevices 
                .First(it => ServicedLocationsContains(it.Value, slotsToExpose));

            return potentialDevice.Key;
        }

        public IDevice GetDeviceByDeviceId(DeviceId deviceId)
        {
            return _transferDevices.FirstOrDefault(device => device.Key.DeviceId.Equals(deviceId)).Key;
        }

        private bool ServicedLocationsContains(List<ServicedLocation> locations, Transfer transfer)
        {
            return locations.Any(location => location.PlcId == transfer.sourceLocation.plcId);
        }
        
        private bool ServicedLocationsContains(List<ServicedLocation> locations, PrepareForPicking prepareForPicking)
        {
            return locations.Any(location => location.PlcId == prepareForPicking.Location.plcId);
        }
        
        private bool ServicedLocationsContains(List<ServicedLocation> locations, SlotsToExpose slotsToExpose)
        {
            return locations.Any(location => location.PlcId == slotsToExpose.Location.plcId);
        }

        public ITransferCompletingDevice ChooseDeviceForTransferDone(ToteTransferRequestDoneModel requestDoneModel)
        {
            var deviceExists = _transferCompletingDevices.Any(it => it.ShouldHandleTransferDone(requestDoneModel));

            if (!deviceExists) throw new NoDeviceCanHandleTransferDoneException() {RequestDoneModel = requestDoneModel};

            var potentialDevice =
                _transferCompletingDevices.First(it => it.ShouldHandleTransferDone(requestDoneModel));

            return potentialDevice;
        }

        public Transfer GetRequestedTransferForRequestDone(ToteTransferRequestDoneModel toteRequestDone)
        {
            try
            {
                var device = ChooseDeviceForTransferDone(toteRequestDone);
                return device?.GetCompletedTransfer(toteRequestDone);
            }
            catch (NoDeviceCanHandleTransferDoneException ex)
            {
                _logger.LogTrace(ex, "Requested transfer not found on any device");
                return null;
            }
            
        }
    }
}