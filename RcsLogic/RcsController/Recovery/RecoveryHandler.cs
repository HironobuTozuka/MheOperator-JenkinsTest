using Common;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Crane;
using RcsLogic.Models;
using RcsLogic.Services;
using SQLitePCL;

namespace RcsLogic.RcsController.Recovery
{
    public class RecoveryHandler
    {
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private readonly LocationService _locationService;
        private readonly TaskBundleService _taskBundleService;
        private readonly UnknownToteRouter _unknownToteRouter;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly RoutingService _routingService;
        private readonly ILogger _logger;

        public RecoveryHandler(LocationRepository locationRepository, ToteRepository toteRepository,
            LocationService locationService, TaskBundleService taskBundleService,
            UnknownToteRouter unknownToteRouter, ILoggerFactory loggerFactory,
            IStoreManagementClient storeManagementClient,
            RoutingService routingService)
        {
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _locationService = locationService;
            _taskBundleService = taskBundleService;
            _unknownToteRouter = unknownToteRouter;
            _loggerFactory = loggerFactory;
            _storeManagementClient = storeManagementClient;
            _routingService = routingService;
            _logger = loggerFactory.CreateLogger<RecoveryHandler>();
        }

        public IRecoveryStrategy Strategy(SystemSortCode systemSortCode)
        {
            _logger.LogDebug("Getting recovery strategy for: {1}", systemSortCode);

            switch (systemSortCode.FailReason)
            {
                case SystemFailReason.ToteOnPlatform:
                    _logger.LogDebug("Returning UnknownToteOnPlatformRecovery for: {1}", systemSortCode);
                    return new UnknownToteOnPlatformRecovery(_locationRepository, _loggerFactory);
                case SystemFailReason.Pick:
                    _logger.LogDebug("Returning TotePickingFailedRecovery for: {1}", systemSortCode);
                    return new TotePickingFailedRecovery(_taskBundleService, _loggerFactory);
                case SystemFailReason.NoTote:
                    _logger.LogDebug("Returning NoToteOnSourceRecovery for: {1}", systemSortCode);
                    return new NoToteOnSourceRecovery(_taskBundleService, _loggerFactory, _toteRepository, _storeManagementClient);
                case SystemFailReason.Overfill:
                    _logger.LogDebug("Returning ToteOverfillRecovery for: {1}", systemSortCode);
                    return new ToteOverfillRecovery(_taskBundleService, _loggerFactory, _locationRepository, _toteRepository, _locationService, _routingService);
                case SystemFailReason.NoRead:
                    _logger.LogDebug("Returning NoReadToteOnCraneRecovery for: {1}", systemSortCode);
                    return new NoReadToteOnCraneRecovery(_taskBundleService, _loggerFactory, _locationRepository, _toteRepository);
                case SystemFailReason.Place:
                case SystemFailReason.PlaceLocationOccupied:
                    _logger.LogDebug("Returning TotePlacingFailedRecovery for: {1}", systemSortCode);
                    return new TotePlacingFailedRecovery(_toteRepository,
                        _locationRepository,
                        _locationService,
                        _unknownToteRouter,
                        _taskBundleService,
                        _loggerFactory);
                default:
                    _logger.LogDebug("Returning TotePickingFailedRecovery for: {1}", systemSortCode);
                    return new TotePickingFailedRecovery(_taskBundleService, _loggerFactory);
                return new UnknownToteOnPlatformRecovery(_locationRepository, _loggerFactory);
            }
        }

        public IRequestTimeoutStrategy Strategy(TimedOutTransfer timedOutTransfer)
        {
            return timedOutTransfer.device is CraneDevice ? new CraneRequestTimeoutRecovery(_taskBundleService, _loggerFactory) : null;
        }
    }
}