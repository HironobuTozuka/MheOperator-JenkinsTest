using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.Services;

namespace RcsLogic.Watchdog
{
    public class ToteLocationWatchdog : IWatchdog
    {
        private readonly ToteRepository _toteRepository;
        private readonly IReturnToteHandler _returnToteHandler;
        private readonly ILogger<ToteLocationWatchdog> _logger;
        private readonly RoutingService _routingService;

        private readonly TimeSpan _toteNoLocationUpdateTimeout;
        private readonly TimeSpan _loadingGateToteTimeout;
        private readonly LocationService _locationService;
        private readonly LocationRepository _locationRepository;
        private readonly IPlcService _plcService;
        public bool Enabled { get; set; } = true;

        public ToteLocationWatchdog(
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            ToteRepository toteRepository, 
            IReturnToteHandler returnToteHandler,
            RoutingService routingService, 
            LocationService locationService, 
            LocationRepository locationRepository,
            IPlcService plcService)
        {
            _logger = loggerFactory.CreateLogger<ToteLocationWatchdog>();
            _toteRepository = toteRepository;
            _returnToteHandler = returnToteHandler;
            _routingService = routingService;
            _locationService = locationService;
            _locationRepository = locationRepository;
            _plcService = plcService;
            Enabled = configuration["Watchdog:ToteLocationWatchdog:Enabled"]?.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase) ?? true;
            var timeout = configuration["Watchdog:ToteLocationWatchdog:ToteNoLocationUpdateTimeout"];
            _toteNoLocationUpdateTimeout = string.IsNullOrEmpty(timeout) ? TimeSpan.FromMinutes(10) : TimeSpan.FromMinutes(int.Parse(timeout));
            timeout = configuration["Watchdog:ToteLocationWatchdog:LoadingGateToteTimeout"];
            _loadingGateToteTimeout = string.IsNullOrEmpty(timeout) ? TimeSpan.FromMinutes(5) : TimeSpan.FromMinutes(int.Parse(timeout));
        }
        
        public void Execute()
        {
            var timedOutTotes = GetTimedOutTotes();
            if(Enabled && timedOutTotes.Any() && _plcService.IsPlcInExecute())
            {
                _logger.LogDebug("Moving back timed out totes {0}", string.Join(";", timedOutTotes));
                timedOutTotes.ForEach(MoveBack);
            }
        }

        public void MoveBackTimedOutTotes()
        {
            _logger.LogDebug("Moving back timed out totes");
            GetTimedOutTotes().ForEach(MoveBack);
        }

        private List<Tote> GetTimedOutTotes()
        {
            var timedOutTotes = _toteRepository.GetTotesNotOnStorageLocation()
                .Where(tote => !tote.location.IsRackingLocation)
                .Where(IsNotOnRobotPlaceLocation())
                .Where(LocationIsRoutedByMhe())
                .Where(TimeoutElapsed())
                .ToList();
            _logger.LogDebug("Timed out totes: {0}", string.Join(";", timedOutTotes));
            return timedOutTotes;
        }

        private Func<Tote, bool> TimeoutElapsed()
        {
            return tote =>
            {
                if (tote.location.zone.function == LocationFunction.LoadingGate)
                {
                    return (DateTime.Now - tote.lastLocationUpdate) > _loadingGateToteTimeout;
                }
                else
                {
                    return (DateTime.Now - tote.lastLocationUpdate) > _toteNoLocationUpdateTimeout;
                }
            };
        }

        private Func<Tote, bool> LocationIsRoutedByMhe()
        {
            return tote => _routingService.IsRoutedByMhe(tote.location);
        }

        private static Func<Tote, bool> IsNotOnRobotPlaceLocation()
        {
            return tote => tote.location.zone.function != LocationFunction.Place;
        }

        private void MoveBack(Tote tote)
        {
            if (StorageLocationIsNullOrLoadingGate(tote))
            {
                _logger.LogDebug("Assigning technical storage location for tote {0}",tote);
                var technicalZone = _locationRepository.GetZoneByFunction(LocationFunction.Technical);
                _locationService._AssignToteTo(technicalZone.zoneId, tote);
                _toteRepository.UpdateToteStatus(tote, ToteStatus.ZoneNotAssigned);
            }
            _logger.LogWarning("Moving tote {0} back to storage. Timeout on location {1}", tote, tote.location);
            try
            {
                _returnToteHandler.ReturnTote(tote.toteBarcode);
            }
            catch (NoDeviceCanHandleTransferException ex)
            {
                _logger.LogError(ex,"No device could handle transfer for timed out tote");
            }
        }

        private static bool StorageLocationIsNullOrLoadingGate(Tote tote)
        {
            return tote.storageLocation == null || tote.storageLocation.zone.function == LocationFunction.LoadingGate;
        }
    }
}