using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;

namespace Data.Seed
{
    public static class Routes
    {
        public static List<Route> Seed(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>();
            routes.AddRange(SeedConveyorRoutes(locations, locationGroups));
            routes.AddRange(SeedCraneARoutes(locations, locationGroups));
            routes.AddRange(SeedCraneBRoutes(locations, locationGroups));
            routes.AddRange(SeedAutomaticRoutes(locations, locationGroups));
            routes.AddRange(SeedLoadingGateRoutes(locations, locationGroups));
            return routes;

        }
        
        private static List<Route> SeedLoadingGateRoutes(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>
            {
                new Route()
                {
                    id = 1,
                deviceId = "LOAD1",
                    locationId = GetLocationId(locations, "LOAD1_2"),
                    routedLocationId = GetLocationId(locations, "LOAD1_3"),
                    isDefaultRoute = true,
                    routeCost = 1
                }
            };

            return routes;
        }

        private static List<Route> SeedAutomaticRoutes(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>
            {
                new Route()
                {
                    id = 2,
                deviceId = "PLC",
                    locationId = GetLocationId(locations, "LOAD1_1"),
                    routedLocationId = GetLocationId(locations, "LOAD1_2"),
                    isDefaultRoute = true,
                    routeCost = 1
                },
                new Route()
                {
                    id = 3,
                deviceId = "PLC",
                    locationId = GetLocationId(locations, "LOAD1_3"),
                    routedLocationId = GetLocationId(locations, "LOAD1_4"),
                    isDefaultRoute = true,
                    routeCost = 1
                }
            };

            return routes;
        }

        private static List<Route> SeedCraneARoutes(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>();
            
            routes.Add(new Route()
            {
                id = 4,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 5,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 6,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 7,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 8,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 9,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 10,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 11,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 12,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 13,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 14,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = false,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 15,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = false,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 16,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = false,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 17,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 18,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 19,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 98,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                isDefaultRoute = true, routeCost = 2
            });
            routes.Add(new Route()
            {
                id = 20,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 21,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 22,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"),
                isDefaultRoute = true, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 23,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 24,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 25,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 26,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 27,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 90,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 28,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 29,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CA_P1"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 30,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CA_P1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 31,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CA_P1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 32,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "CA_P2"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 33,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CA_P2"),
                routedLocationId = GetLocationId(locations, "CNV1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 34,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CA_P2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 35,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CA_P2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                isDefaultRoute = true, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 36,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 37,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 38,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CNV2_1"),
                routedLocationId = GetLocationId(locations, "CA_P2"), isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 39,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CA_P2"),
                routedLocationId = GetLocationId(locations, "CA_P2"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 40,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CA_P1"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 41,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "CA_P1"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 42,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "CA_P2"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 43,
                deviceId = "CA_P1", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 91,
                deviceId = "CA_P2", locationId = GetLocationId(locations, "LOAD1_4"),
                routedLocationId = GetLocationId(locations, "LOAD1_1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 97,
                deviceId = "CA_P2", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 100,
                deviceId = "CA_P2", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 101,
                deviceId = "CA_P2", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"), isDefaultRoute = false, routeCost = 3
            });
            routes.Add(new Route()
            {
                id = 102,
                deviceId = "CA_P2", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"), isDefaultRoute = false, routeCost = 3
            });
            routes.Add(new Route()
            {
                id = 103,
                deviceId = "CA_P1", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"), isDefaultRoute = false, routeCost = 3
            });
            routes.Add(new Route()
            {
                id = 104,
                deviceId = "CA_P1", locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"), isDefaultRoute = false, routeCost = 3
            });
            
                        
            routes.Add(new Route()
            {
                id = 105,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_1"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 106,
                deviceId = "CA_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "CA_P1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 107,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_2"),
                routedLocationId = GetLocationId(locations, "CA_P2"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 108,
                deviceId = "CA_P2",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackA_3"),
                routedLocationId = GetLocationId(locations, "CA_P2"), isDefaultRoute = true,
                routeCost = 1
            });

            return routes;
        }
        
        private static List<Route> SeedCraneBRoutes(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>();
            
            routes.Add(new Route()
            {
                id = 44,
                deviceId = "CB_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = true, routeCost = 2
            });
            routes.Add(new Route()
            {
                id = 45,
                deviceId = "CB_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationId = GetLocationId(locations, "CNV2_5"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 46,
                deviceId = "CB_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 47,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 48,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = true, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 49,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId = GetLocationId(locations, "CNV2_5"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 50,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 51,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 52,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER2"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 53,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER1"),
                routedLocationId = GetLocationId(locations, "CNV2_5"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 54,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER2"),
                routedLocationId = GetLocationId(locations, "CNV2_5"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 55,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER1"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 56,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER2"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 57,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId = GetLocationId(locations, "ORDER1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 58,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId = GetLocationId(locations, "ORDER2"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 59,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId = GetLocationId(locations, "ORDER1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 60,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId = GetLocationId(locations, "ORDER2"), isDefaultRoute = true,
                routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 61,
                deviceId = "CB_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationId = GetLocationId(locations, "ORDER1"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 62,
                deviceId = "CB_P1",
                locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationId = GetLocationId(locations, "ORDER2"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 63,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationId = GetLocationId(locations, "CB_P1"), isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 64,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 65,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationId = GetLocationId(locations, "CNV2_5"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 66,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationId = GetLocationId(locations, "ORDER1"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 67,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationId = GetLocationId(locations, "ORDER2"), isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 68,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CB_P1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 69,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "RPP1"),
                routedLocationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 70,
                deviceId = "CB_P1", locationTypeId = GetLocationGroupId(locationGroups,  "RackB"),
                routedLocationId = GetLocationId(locations, "RPP1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 71,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId = GetLocationId(locations, "RPP1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 72,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId = GetLocationId(locations, "RPP1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 73,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "RPP1"),
                routedLocationId = GetLocationId(locations, "CNV2_5"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 74,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "RPP1"),
                routedLocationId =  GetLocationId(locations, "ORDER1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 75,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "RPP1"),
                routedLocationId =  GetLocationId(locations, "ORDER2"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 76,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER1"),
                routedLocationId =  GetLocationId(locations, "RPP1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 77,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER2"),
                routedLocationId =  GetLocationId(locations, "RPP1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 92,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 93,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 94,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "RPP1"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 95,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER1"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 96,
                deviceId = "CB_P1", locationId = GetLocationId(locations, "ORDER2"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 99,
                deviceId = "CB_P1", locationTypeId = GetLocationGroupId(locationGroups, "RackB"),
                routedLocationId =  GetLocationId(locations, "CB_P1"),
                isDefaultRoute = true, routeCost = 1
            });

            return routes;
        }

        private static List<Route> SeedConveyorRoutes(List<Location> locations, List<LocationGroup> locationGroups)
        {
            var routes = new List<Route>();
            
            routes.Add(new Route()
            {
                id = 78,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1"),
                routedLocationId = GetLocationId(locations, "CNV1_3"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 79,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1_3"),
                routedLocationId = GetLocationId(locations, "CNV1_4"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 80,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1_3"),
                routedLocationId = GetLocationId(locations, "CNV3_2"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 81,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1_4"),
                routedLocationId = GetLocationId(locations, "CNV1_5"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 82,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1_4"),
                routedLocationId = GetLocationId(locations, "CNV4_2"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 83,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV2_3"),
                routedLocationId = GetLocationId(locations, "CNV2_2"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 84,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV2_2"),
                routedLocationId = GetLocationId(locations, "CNV2_1"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 85,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV2_4"),
                routedLocationId = GetLocationId(locations, "CNV2_3"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 86,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV2_5"),
                routedLocationId = GetLocationId(locations, "CNV2_4"),
                isDefaultRoute = false, routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 87,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV1_5"),
                routedLocationId = GetLocationId(locations, "CNV1_4"),
                isDefaultRoute = false, routeCost = 1
            });

            routes.Add(new Route()
            {
                id = 88,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV3_2"),
                routedLocationId = GetLocationId(locations, "CNV2_3"), isDefaultRoute = true,
                routeCost = 1
            });
            routes.Add(new Route()
            {
                id = 89,
                deviceId = "CNV", locationId = GetLocationId(locations, "CNV4_2"),
                routedLocationId = GetLocationId(locations, "CNV2_4"), isDefaultRoute = true,
                routeCost = 1
            });

            return routes;
        }

        private static int GetLocationId(List<Location> locations, string plcId)
        {
            return locations.First(location => location.plcId == plcId).id;
        }

        private static int GetLocationGroupId(List<LocationGroup> locationGroups, string groupId)
        {
            return locationGroups.First(locationType => locationType.name == groupId).id;
        }
    }
}