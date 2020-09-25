using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class Cranes
    {
        public static List<Location> Seed(ref int index)
        {
            var locations = new List<Location>();
            const string craneLocationZone = "CRANE";

            var cap1 = new Location()
            {
                locationHeight = 400, plcId = "CA_P1", isBackLocation = false, rack = "A", locationGroup = null,
                zoneId = craneLocationZone,
                id = index++
            };
            locations.Add(cap1);
            var cap2 = new Location()
            {
                locationHeight = 400, plcId = "CA_P2", isBackLocation = false, rack = "A", locationGroup = null,
                zoneId = craneLocationZone,
                id = index++
            };
            locations.Add(cap2);
            var cbp1 = new Location()
            {
                locationHeight = 400, plcId = "CB_P1", isBackLocation = false, rack = "B", locationGroup = null,
                zoneId = craneLocationZone,
                id = index++
            };
            locations.Add(cbp1);
            return locations;
        }
    }
}