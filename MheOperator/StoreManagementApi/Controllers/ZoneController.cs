using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;
using MheOperator.StoreManagementApi.Models;
using MheOperator.StoreManagementApi.Models.Zone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RcsLogic.Services;

namespace MheOperator.StoreManagementApi.Controllers
{
    [Route("api/internal")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly ILogger<ZoneController> _logger;
        private readonly LocationService _locationService;

        public ZoneController(ILogger<ZoneController> logger, LocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        /// <summary>
        /// Retrieves zone availability
        /// </summary>
        /// <returns></returns>
        [HttpGet("zone:list")]
        public ZoneList GetZoneList()
        {
            return new ZoneList()
            {
                Zones = _locationService.GetZonesEmptyLocationCounts()
                    .Select(zoneCount => new ZoneInfo()
                    {
                        ZoneId = zoneCount.Key.id,
                        Functions = new List<LocationFunction>() {zoneCount.Key.function},
                        TempRegime = zoneCount.Key.temperatureRegime,
                        AvailableLocations = zoneCount.Value
                    }).ToList()
            };
        }
    }
}