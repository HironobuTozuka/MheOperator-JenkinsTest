using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;

namespace Data.Seed
{
    public static class RackBLocations
    {
        public static List<Location> Seed(ref int index)
        {
            var locations = new List<Location>();
            
            var rackBLocations = new List<Location>();
            const int rackBLocationGroup = 5;
            const string ambientZone = "AMBIENT";
            const string stagingZone = "STAGING";

            //Create locations on RACK_B
            for (var rack = 1; rack <= 2; rack++)
            {
                for (var col = 1; col <= 3; col++)
                {
                    for (var row = 1; row <= 11; row++)
                    {
                        if (SkipOrderGateLocations(col, row, rack))
                        {
                            continue;
                        }

                        if (SkipRack1LocationsInRobotArea(row, rack))
                        {
                            continue;
                        }

                        var loc = new Location();
                        loc.id = index++;
                        loc.locationGroupId = rackBLocationGroup;
                        loc.locationHeight = 120;
                        var colStr = col.ToString();
                        if (colStr.Length < 2) colStr = "0" + colStr;
                        var shelfStr = row.ToString();
                        if (shelfStr.Length < 2) shelfStr = "0" + shelfStr;
                        loc.rack = "B" + rack.ToString();
                        loc.col = col;
                        loc.row = row;
                        loc.plcId = loc.rack + "L" + colStr + shelfStr + "F";
                        loc.status = LocationStatus.Enabled ;
                        loc.zoneId = ambientZone;
                        if (row < 6) loc.zoneId = stagingZone;

                        loc.isBackLocation = false;
                        rackBLocations.Add(loc);

                        var backLoc = new Location();
                        backLoc.id = index++;
                        backLoc.locationGroupId = loc.locationGroupId;
                        backLoc.locationHeight = loc.locationHeight;
                        backLoc.rack = loc.rack;
                        backLoc.col = loc.col;
                        backLoc.row = loc.row;
                        backLoc.plcId = "B" + rack.ToString() + "L" + colStr + shelfStr + "B";
                        backLoc.isBackLocation = true;
                        backLoc.frontLocationId = loc?.id;
                        backLoc.status = loc.status;
                        backLoc.zoneId = ambientZone;
                        rackBLocations.Add(backLoc);
                    }
                }
            }

            locations.AddRange(rackBLocations);
            return locations;
        }

        private static bool SkipRack1LocationsInRobotArea(int row, int rack)
        {
            return row < 9 && rack == 1;
        }

        private static bool SkipOrderGateLocations(int col, int row, int rack)
        {
            return (col == 1 || col == 3) && (row == 2) && rack == 2;
        }
    }
}