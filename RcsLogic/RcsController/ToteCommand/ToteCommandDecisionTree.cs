using System.Linq;
using Common;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Services;

namespace RcsLogic.RcsController.ToteCommand
{
    public class ToteCommandDecisionTree
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly ILoggerFactory _loggerFactory;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly RoutingService _routingService;
        private readonly ToteRepository _toteRepository;
        private readonly UnknownToteRouter _unknownToteRouter;
        private readonly LocationRepository _locationRepository;
        private readonly ILogger<ToteCommandDecisionTree> _logger;

        public ToteCommandDecisionTree(
            ILoggerFactory loggerFactory,
            TaskBundleService taskBundleService,
            DeviceRegistry deviceRegistry,
            IStoreManagementClient storeManagementClient, 
            RoutingService routingService, 
            ToteRepository toteRepository,
            UnknownToteRouter unknownToteRouter,
            LocationRepository locationRepository)
        {
            _taskBundleService = taskBundleService;
            _loggerFactory = loggerFactory;
            _deviceRegistry = deviceRegistry;
            _storeManagementClient = storeManagementClient;
            _routingService = routingService;
            _toteRepository = toteRepository;
            _unknownToteRouter = unknownToteRouter;
            _locationRepository = locationRepository;
            _logger = loggerFactory.CreateLogger<ToteCommandDecisionTree>();
        }

        public IToteCommand CreateCommand(Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            var commandSet = new ToteCommandSet();

            if (ShouldWaitOnLoadingGate(scanLocation, tote))
            {
                commandSet.Add(new MakeToteOnLocationReadyCommand(_loggerFactory, tote, _toteRepository));
                _logger.LogTrace("Added MakeToteOnLocationReadyCommand to commandSet");
                commandSet.Add(new NotifySMToteOnLocationCommand(_storeManagementClient, scanLocation, tote, toteRotation));
                _logger.LogTrace("Added NotifySMToteOnLocationCommand to commandSet");
            }

            if (NoToteOnLoadingGate(scanLocation, tote))
            {
                commandSet.Add(new NotifySMToteOnLocationCommand(_storeManagementClient, scanLocation, tote, toteRotation));
                _logger.LogTrace("Added NotifySMToteOnLocationCommand to commandSet");
            }
            
            if (ShouldExposeSlot(tote))
            {
                commandSet.Add(new ExposeSlotCommand(_taskBundleService, _deviceRegistry, scanLocation, tote, toteRotation));
                _logger.LogTrace("Added ExposeSlotCommand to commandSet");
            }

            if (commandSet.HasAnyCommands()) return commandSet;
            _logger.LogTrace("commandSet was empty");
            
            if (Barcode.IsWrongBarcode(tote.toteBarcode))
            {
                _logger.LogTrace("Returning RouteUnknownToteCommand");
                return new RouteUnknownToteCommand(_loggerFactory, _unknownToteRouter, _deviceRegistry, scanLocation, tote, toteRotation);
            }

            if (ShouldNotifyRobot(scanLocation, tote))
            {
                _logger.LogTrace("Returning NotifyRobotCommand");
                return new NotifyRobotCommand(_loggerFactory, _deviceRegistry, scanLocation, tote, toteRotation);
            }

            _logger.LogTrace("Returning RouteToteCommand");
            return  new RouteToteCommand(_loggerFactory, _taskBundleService,
                _routingService, _deviceRegistry, _toteRepository, _locationRepository, scanLocation, tote, toteRotation);
        }

        private bool NoToteOnLoadingGate(Location scanLocation, Tote tote)
        {
            return scanLocation.zone.function == LocationFunction.LoadingGate && 
                   tote.toteBarcode.Equals(Barcode.NoTote);
        }

        public IToteCommand CreateReturnCommand(Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            return  new RouteToteCommand(_loggerFactory, _taskBundleService,
                _routingService, _deviceRegistry, _toteRepository, _locationRepository, scanLocation, tote, toteRotation);
        }
        
        private bool ShouldWaitOnLoadingGate(Location scanLocation, Tote tote)
        {
            return scanLocation.zone.function == LocationFunction.LoadingGate && 
                   (!Barcode.IsWrongBarcode(tote.toteBarcode) || tote.toteBarcode.Contains(Barcode.NoRead));
        }
        
        private bool ShouldExposeSlot(Tote tote)
        {
            return _taskBundleService.GetDeliverTask(tote.toteBarcode) != null &&
                tote.location?.IsGateLocation == true;
        }
        
        private bool ShouldNotifyRobot(Location scanLocation, Tote tote)
        {
            return IsRobotLocation(scanLocation) && ToteExistsInTwoFirstPickTaskBundles(tote)
                   || IsRobotPlaceLocation(scanLocation);
        }
        
        private static bool IsRobotLocation(Location location)
        {
            return location.zone.function == LocationFunction.Pick
                   || location.zone.function == LocationFunction.Place;
        }
        
        private static bool IsRobotPlaceLocation(Location location)
        {
            return location.zone.function == LocationFunction.Place;
        }
        
        private bool ToteExistsInTwoFirstPickTaskBundles(Tote tote)
        {
            var currentTaskBundle = _taskBundleService.GetCurrentPickTaskBundle();
            var nextPickTaskBundle = _taskBundleService.GetNextPickTaskBundle();

            return currentTaskBundle != null && currentTaskBundle.tasks.OfType<PickTask>()
                       .Where(task => !task.IsFinished())
                       .Any(task =>
                           task.sourceTote.toteId.Equals( tote.toteBarcode) || task.destTote.toteId.Equals(tote.toteBarcode)) ||
                   nextPickTaskBundle != null && nextPickTaskBundle.tasks.OfType<PickTask>()
                       .Where(task => !task.IsFinished())
                       .Any(task =>
                           task.sourceTote.toteId.Equals(tote.toteBarcode));
        }
    }
}