using System;
using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class LoadingGate
    {
        public static List<Location> Seed(ref int index)
        {
            var locations = new List<Location>();
            
            const string conveyorLocationZone = "LOADING_GATE_CNV";
            const string loadingGateLocationZone = "LOADING_GATE";

            for (var i = 1; i <= 4; i++)
            {
                var loc = new Location();
                loc.id = index++;
                loc.locationHeight = 170;
                loc.col = (int) Math.Ceiling((double) i / 2.0);
                if (i > 2) loc.row = 1 + i % 2;
                else loc.row = 2 - i % 2;
                loc.zoneId = conveyorLocationZone;
                if (i == 2) loc.zoneId = loadingGateLocationZone;
                var colStr = i.ToString();
                loc.plcId = loc.rack + "LOAD1_" + colStr;

                loc.isBackLocation = false;
                loc.status = LocationStatus.Enabled ;

                locations.Add(loc);
            }

            return locations;
        }
    }
}