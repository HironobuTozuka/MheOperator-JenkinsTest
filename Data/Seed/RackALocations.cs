using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;

namespace Data.Seed
{
    public static class RackALocations
    {
        public static List<Location> Seed(ref int index)
        {
            var locations = new List<Location>();
            
            var rackALocations = new List<Location>();
            const int rackA1LocationType = 1;
            const int rackA2LocationType = 2;
            const int rackA3LocationType = 3;
            const string locationZone = "CHILL";
            const string technicalZone = "TECHNICAL";
            //Create locations on RACK_A
            for (var rack = 1; rack <= 2; rack++)
            {
                for (var col = 1; col <= 8; col++)
                {
                    for (var row = 1; row <= 10; row++)
                    {
                        if (SkipLoadingGateLocation(col, row, rack))
                        {
                            continue;
                        }

                        if (SkipLocationOverRobot(col, row))
                        {
                            continue;
                        }

                        if (SkipLocationOverConveyor(col, row))
                        {
                            continue;
                        }
                        var colStr = col.ToString();
                        var loc = new Location();

                        if (col <= 3)
                        {
                            loc.locationGroupId = rackA1LocationType;
                            
                        }
                        else if (col <= 6)
                        {
                            loc.locationGroupId = rackA2LocationType;
                        }
                        else
                        {
                            loc.locationGroupId = rackA3LocationType;
                        }
                        loc.id = index++;

                        loc.locationHeight = row > 4 ? 120 : 170;

                        if (LocationIsUnderOrOverLoadingGate(row, col)) loc.locationHeight = 120;
                        if (colStr.Length < 2) colStr = "0" + colStr;
                        var shelfStr = row.ToString();
                        if (shelfStr.Length < 2) shelfStr = "0" + shelfStr;
                        loc.rack = "A" + rack.ToString();
                        loc.col = col;
                        loc.row = row;
                        loc.plcId = loc.rack + "L" + colStr + shelfStr;
                        loc.zoneId = locationZone;
                        if(row == 10 && col >= 3 && col <= 6) loc.zoneId = technicalZone;
                        if(row == 10) loc.locationHeight = 180;
                        
                        loc.isBackLocation = false;
                        loc.status = LocationStatus.Enabled ;

                        rackALocations.Add(loc);
                    }
                }
            }

            locations.AddRange(rackALocations);

            return locations;
        }

        private static bool LocationIsUnderOrOverLoadingGate(int row, int col)
        {
            return row < 4 && (col == 5 || col == 6);
        }

        private static bool SkipLocationOverConveyor(int col, int row)
        {
            return col < 5 && row < 3;
        }

        private static bool SkipLocationOverRobot(int col, int row)
        {
            return col < 2 && row < 10;
        }

        private static bool SkipLoadingGateLocation(int col, int row, int rack)
        {
            return (col == 5 || col == 6) && (row == 2) && rack == 1;
        }
    }
}