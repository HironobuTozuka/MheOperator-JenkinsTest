using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class ConveyorLocations
    {
        public static List<Location> Seed(ref int index)
        {
            var locations = new List<Location>();
            const string conveyorLocationZone = "CONVEYOR";
            const string pickLocationZone = "PICK";
            const string placeLocationZone = "PLACE";
            //Create locations on conveyors

            for (int row = 1; row <= 3; row++)
            {
                for (int col = 1; col <= 5; col++)
                {
                    if (row == 2 && (col < 3 || col > 4)) continue;

                    Location loc = new Location();
                    if (row == 2 || col > 4 && row == 3) loc.zoneId = pickLocationZone;
                    else loc.zoneId = conveyorLocationZone;
                    loc.locationHeight = 400;
                    loc.id = index++;

                    loc.col = col;
                    loc.row = row;

                    string colStr;
                    string rowStr;

                    if (row == 2)
                    {
                        colStr = 2.ToString();
                        rowStr = col.ToString();
                    }
                    else
                    {
                        colStr = col.ToString();
                        rowStr = (row).ToString();
                        if (row == 3) rowStr = (row - 1).ToString();
                    }

                    loc.plcId = loc.rack + "CNV" + rowStr + "_" + colStr;

                    loc.isBackLocation = false;
                    loc.status = LocationStatus.Enabled ;
                    locations.Add(loc);
                }
            }

            var loc2 = new Location()
            {
                id = index++,
                plcId = "CNV1",
                zoneId = conveyorLocationZone,
                locationHeight = 400
            };
            var loc3 = new Location()
            {
                id = index++,
                plcId = "RPP1",
                zoneId = placeLocationZone,
                locationHeight = 400,
                col = 6,
                row = 1
            };
            locations.Add(loc2);
            locations.Add(loc3);
            return locations;
        }
    }
}