using System.Collections.Generic;
using Common.Models.Location;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models.Zone
{
    /// <summary>
    /// Describes zone location availability and it's functions
    /// </summary>
    public class ZoneInfo
    {
        /// <summary>
        /// Name of the zone
        /// </summary>
        public string ZoneId { get; set; }

        /// <summary>
        /// Temperature regime of the zone
        /// </summary>
        public TemperatureRegime TempRegime { get; set; }

        /// <summary>
        /// Functions assigned to the zone
        /// </summary>
        public List<LocationFunction> Functions { get; set; }

        /// <summary>
        /// Number of available empty locations
        /// </summary>
        public int AvailableLocations { get; set; }
    }
}