using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;

namespace RcsLogic.Crane
{
    public class BlockedTotesSolver
    {
        private readonly ILogger<BlockedTotesSolver> _logger;
        private readonly ToteRepository _toteRepository;
        private readonly LocationRepository _locationRepository;
        private readonly LocationService _locationService;
        private readonly TransferCollection _transfers;
        private readonly DeviceId _deviceId;
        private readonly TaskBundleService _taskBundleService;

        public BlockedTotesSolver(ILoggerFactory loggerFactory,
            ToteRepository toteRepository,
            LocationRepository locationRepository,
            LocationService locationService,
            TransferCollection transfers,
            DeviceId deviceId, TaskBundleService taskBundleService)
        {
            _logger = loggerFactory.CreateLogger<BlockedTotesSolver>();
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            _locationService = locationService;
            _transfers = transfers;
            _deviceId = deviceId;
            _taskBundleService = taskBundleService;
        }

        public List<Transfer> ShuffleIfRequired(List<Transfer> requests)
        {
            var blockingTotes = new List<Tote>();
            requests.Select(request => GetObstructingTotes(request.sourceLocation, request.destLocation)).ToList()
                .ForEach(
                    toteList =>
                    {
                        if (toteList != null) blockingTotes.AddRange(toteList);
                    });
            if (blockingTotes.Any())
            {
                requests = MoveBlockingTotes(blockingTotes);
            }

            return requests;
        }

        private List<Tote> GetObstructingTotes(Location source, Location dest)
        {
            if (source.isBackLocation || dest.isBackLocation)
            {
                var frontTotes = new List<Tote>();
                if (source.isBackLocation && source.frontLocationId != null)
                {
                    var tote = _toteRepository.GetToteOnLocation((int) source.frontLocationId);
                    if (tote != null) frontTotes.Add(tote);
                }

                if (dest.isBackLocation && dest.frontLocationId != null)
                {
                    var tote = _toteRepository.GetToteOnLocation((int) dest.frontLocationId);
                    if (tote != null) frontTotes.Add(tote);
                }

                return frontTotes;
            }
            else
            {
                return null;
            }
        }

        private List<Transfer> MoveBlockingTotes(List<Tote> blockingTotes)
        {
            var unblockingRequests = blockingTotes.Select(tote =>
            {
                var destLoc = _locationRepository.GetShuffleLocation(tote.location);
                _logger.LogInformation("{0} Found shuffle location {1} for tote {2}", _deviceId, destLoc, tote);
                if (destLoc == null)
                {
                    _logger.LogError("{0} Not enough empty locations!!!", _deviceId);
                    throw new System.Exception("Not enough empty locations!!!");
                }

                _toteRepository.UpdateToteStorageLocation(tote, destLoc);
                var moveTask = _taskBundleService.GetInternalMoveTask(tote, destLoc);
                return new Transfer()
                {
                    task = moveTask, tote = tote, sourceLocation = tote.location,
                    destLocation = destLoc
                };
            }).ToList();
            _transfers.AddRange(unblockingRequests);
            return unblockingRequests;
        }
    }
}