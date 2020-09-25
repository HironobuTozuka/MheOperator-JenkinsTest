using System;
using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class OrderGates
    {
        public static List<Location> Seed(ref int index)
        {
            const string order1LocationZone = "ORDER_GATE_1";
            const string order2LocationZone = "ORDER_GATE_2";

            var locations = new List<Location>();
            
            for (var i = 1; i <= 2; i++)
            {
                var loc = new Location();
                loc.id = index++;
                loc.locationHeight = 400;
                loc.col = i;
                var colStr = i.ToString();
                loc.plcId = loc.rack + "ORDER" + colStr;
                loc.zoneId = order1LocationZone;
                if (i > 1) loc.zoneId = order2LocationZone;

                loc.isBackLocation = false;
                loc.status = LocationStatus.Enabled ;

                locations.Add(loc);
            }

            return locations;
        }
    }
}