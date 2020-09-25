using System;
using Common;
using Common.Models;
using Common.Models.Location;
using Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace RcsLogic
{
    public class LocationStatus
    {
        private readonly IPlcService _plcService;
        private readonly ILogger<LocationStatus> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly LocationRepository _locationRepository;

        public LocationStatus(ILoggerFactory loggerFactory, IPlcService plcService, IMemoryCache memoryCache, LocationRepository locationRepository)
        {
            _logger = loggerFactory.CreateLogger<LocationStatus>();
            _plcService = plcService;
            _memoryCache = memoryCache;
            _locationRepository = locationRepository;
        }

        public bool IsLocationOccupied(Location location)
        {
            _logger.LogDebug("Checking location {0} status", location.plcId);
            // if (location?.zone?.function == LocationFunction.Place)
            // {
            //     return _locationRepository.IsOccupied(location);
            // }
            if (!location.plcId.Contains("CNV") && !location.plcId.Contains("ORDER")
                                                && !location.plcId.Contains("LOAD"))
            {
                return _locationRepository.IsOccupied(location);
            }

            return _memoryCache.GetOrCreate<bool>(location.plcId, entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(500);
                return _plcService.IsConveyorOccupied(location.plcId);
            });
        }
    }
}