using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Gate;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Common.Models.Transfer;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;

namespace RcsLogic.Gates
{
    public class LoadingGate : Device, ITaskBundleAddedListener, ITransferDevice, ISlotExposingDevice
    {
        private readonly ILogger<LoadingGate> _logger;

        private readonly IPlcService _plcService;
        private readonly ToteRepository _toteRepository;
        private readonly RoutingService _routingService;
        private readonly List<ServicedLocation> _servicedLocations;
        public List<ServicedLocation> ServicedLocations => _servicedLocations;

        public LoadingGate(DeviceId deviceId,
            IPlcService plcService,
            ILoggerFactory loggerFactory,
            TaskBundleService tasks,
            ServicedLocationProvider servicedLocationProvider,
            ToteRepository toteRepository,
            RoutingService routingService) : base(tasks, deviceId)
        {
            _logger = loggerFactory.CreateLogger<LoadingGate>();
            _servicedLocations = servicedLocationProvider.GetServicedLocations(deviceId);

            _plcService = plcService;
            _toteRepository = toteRepository;
            _routingService = routingService;
        }

        public void HandleNewTaskBundle(TaskBundle newTaskBundle)
        {
            newTaskBundle.tasks.OfType<MoveTask>().ToList().ForEach(HandleMoveTask);
        }

        private void HandleMoveTask(MoveTask moveTask)
        {
            var tote = _toteRepository.GetToteByBarcode(moveTask.toteId);
            if (!IsAtServicedLocation(tote)) return;

            var nextLocation = _routingService.GetNextLocation(tote.location, moveTask.destLocation);
            
            _logger.LogDebug("Requesting tote transfer on loading gate for tote: {0} form: {1} to: {2}", 
                tote.toteBarcode, tote.location.plcId, nextLocation.plcId);

            _toteRepository.UpdateToteStatus(tote, ToteStatus.Ready);
            
            _plcService.RequestTransfer(new TransferRequestModel(
                new ToteTransferRequestModel()
                {
                    ToteBarcode = moveTask.toteId,
                    Id = new TransferId(moveTask.taskId),
                    ToteType = new RequestToteType(tote.type),
                    SourceLocationId = tote.location.plcId,
                    DestLocationId = nextLocation.plcId
                },
                null));

            _taskBundles.UpdateTaskStatus(moveTask.taskId, RcsTaskStatus.Executing);
        }

        private bool IsAtServicedLocation(Tote tote)
        {
            return _servicedLocations.Exists(loc => loc.Id == tote.locationId);
        }

        public void Execute(Transfer request)
        {
            
            _plcService.RequestTransfer(new TransferRequestModel(
                new ToteTransferRequestModel()
                {
                    ToteBarcode = request.tote.toteBarcode,
                    Id = new TransferId(request.task.taskId),
                    ToteType = new RequestToteType(request.tote.type),
                    SourceLocationId = request.sourceLocation.plcId,
                    DestLocationId = request.destLocation.plcId
                },
                null));
        }
        
        public void Expose(SlotsToExpose slotsToExpose)
        {
            _logger.LogInformation("Exposing slots {0}", slotsToExpose);
            
            _plcService.OpenGate(new GateDescription()
                {gateId = Convert(slotsToExpose.Location)});
            
            _taskBundles.CompleteDeliveryTask(slotsToExpose.Tote.toteBarcode);
        }

        public void Execute(List<Transfer> transfers)
        {
            transfers.ForEach(Execute);
        }

        private GateId Convert(Location location)
        {
            return new GateId(location.zone.plcGateId);
        }
    }
}