using System.Collections.Generic;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models.Zone
{
    /// <summary>
    /// Describes all available empty locations in the store
    /// </summary>
    public class ZoneList
    {
        /// <summary>
        /// List of available zones in the RCS store
        /// </summary>
        public List<ZoneInfo> Zones { get; set; }
    }
}