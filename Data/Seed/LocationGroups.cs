using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class LocationGroups
    {
        public static List<LocationGroup> Seed()
        {
            return new List<LocationGroup>()
            {
                new LocationGroup() {id = 1, name = "RackA_1"},
                new LocationGroup() {id = 2, name = "RackA_2"},
                new LocationGroup() {id = 3, name = "RackA_3"},
                new LocationGroup() {id = 5, name = "RackB"},
            };
        }
    }
}