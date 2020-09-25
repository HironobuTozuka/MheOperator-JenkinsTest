using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;
using RcsLogic.Services.Exceptions;

namespace RcsLogic.RcsController.ToteCommand
{
    public class RouteToteCommand : IToteCommand
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly ILogger<RouteToteCommand> _logger;
        private readonly RoutingService _routingService;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly ToteRepository _toteRepository;
        private readonly LocationRepository _locationRepository;

        private readonly Location _scanLocation;
        private readonly Tote _tote;
        private readonly ToteRotation _toteRotation;

        public RouteToteCommand(
            ILoggerFactory loggerFactory,
            TaskBundleService taskBundleService, 
            RoutingService routingService, 
            DeviceRegistry deviceRegistry,
            ToteRepository toteRepository,
            LocationRepository locationRepository,
            Location scanLocation, Tote tote, 
            ToteRotation toteRotation)
        {
            _taskBundleService = taskBundleService;
            _routingService = routingService;
            _deviceRegistry = deviceRegistry;
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            this._scanLocation = scanLocation;
            this._tote = tote;
            this._toteRotation = toteRotation;
            _logger = loggerFactory.CreateLogger<RouteToteCommand>();
        }

        public void Execute()
        {
            var task = GetTaskForTote(_tote);
            var destination = GetDestination(task, _tote);
            if (_scanLocation.Equals(destination))
            {
                _logger.LogInformation($"Skipped processing transfer from {_scanLocation} to {destination}," +
                                       $" tote is already on dest location");
                return;
            }
            
            var transfersToExecute = new List<Transfer>();
            
            task ??= _taskBundleService.GetInternalMoveTask(_tote, destination);

            Location nextLocation;
            try
            {
                nextLocation = _routingService.GetNextLocation(_scanLocation, destination);
            }
            catch(NextRouteLocationIsLocationGroup ex)
            {
                _logger.LogWarning("Received exception on routing, handling by moving to conveyor", ex);
                nextLocation = _routingService.GetNextLocation(_scanLocation, 
                    _locationRepository.GetLocationByFunction(LocationFunction.Conveyor));
            }
            

            transfersToExecute.Add( new Transfer()
            {
                destLocation = nextLocation,
                sourceLocation = _scanLocation,
                tote = _tote,
                task = task
            });
            
            var device = _deviceRegistry.ChooseDeviceForTransfer(transfersToExecute.First());
            
            if (LocationShouldBeVacated(nextLocation))
            {
                var toteToReturn = _toteRepository.GetToteByBarcode(nextLocation.storedTote.toteBarcode);
                var toteToReturnNextLocation = _routingService.GetNextLocation(toteToReturn.location, toteToReturn.storageLocation);
                
                transfersToExecute.Add( new Transfer()
                {
                    destLocation = toteToReturnNextLocation,
                    sourceLocation = toteToReturn.location,
                    tote = toteToReturn,
                    task = _taskBundleService.GetInternalMoveTask(toteToReturn, toteToReturn.storageLocation)
                });
            }

            device.Execute(transfersToExecute);
        }
        
        private TaskBase GetTaskForTote(Tote tote)
        {
            var deliveryMoveTask = _taskBundleService.GetDeliveryMoveTask(tote.toteBarcode);
            if (deliveryMoveTask != null) return deliveryMoveTask;

            var pickTaskBundle = _taskBundleService.GetCurrentPickTaskBundle();
            var pickTask = pickTaskBundle?.tasks.OfType<PickTask>()
                .Where(t => !t.IsFinished())
                .FirstOrDefault(t =>
                    (t.destTote.toteId == tote.toteBarcode || t.sourceTote.toteId == tote.toteBarcode));
            if (pickTask != null) return pickTask;

            var nonDeliveryMoveTask = _taskBundleService.GetNonDeliveryMoveTask(tote.toteBarcode);
            if (nonDeliveryMoveTask != null) return nonDeliveryMoveTask;

            _logger.LogInformation(
                "No Active move tasks found! Creating move task to storage for tote: {0} with dest location: {1}",
                tote.toteBarcode, tote.storageLocation);
            return null;
        }
        
        private Location GetDestination(TaskBase task, Tote tote)
        {
            return task switch
            {
                MoveTask moveTask => moveTask.destLocation,
                PickTask pickTask when pickTask.destTote.toteId.Equals(tote.toteBarcode) => pickTask.destTote
                    .pickLocation,
                PickTask pickTask when pickTask.sourceTote.toteId.Equals(tote.toteBarcode) => pickTask.sourceTote
                    .pickLocation,
                _ => tote.storageLocation
            };
        }
        
        private bool LocationShouldBeVacated(Location location)
        {
            return location.storedTote != null && location.zone.function == LocationFunction.Place;
        }
    }
}