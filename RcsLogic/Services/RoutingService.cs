using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;
using Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RcsLogic.Models.Device;
using RcsLogic.Services.Exceptions;
using RoutingLocationDescription = RcsLogic.Models.RoutingService.RoutingLocationDescription;

namespace RcsLogic.Services
{
    public class RoutingService
    {
        private List<Route> _routings;
        private IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly LocationRepository _locationRepository;

        private struct SourceDest
        {
            public int startLocationId;
            public int destLocationId;
        }

        public RoutingService(IMemoryCache memoryCache,
            IServiceProvider serviceProvider, 
            LocationRepository locationRepository,
            DeviceStatusService deviceStatus)
        {
            _serviceProvider = serviceProvider;
            _locationRepository = locationRepository;
            //var disabledDevices = configuration[]

            StoreDbContext dbContext =
                serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
            _routings = dbContext.routes.ToList()
                .Where(route => deviceStatus.IsEnabled(new DeviceId(route.deviceId))).ToList();
            _memoryCache = memoryCache;
            dbContext.Dispose();
        }

        public bool IsRoutedByMhe(Location source)
        {
            return _routings.Any(routing =>
                routing.locationId == source.id
                || (routing.locationId == null && routing.locationTypeId == source.locationGroupId)
                && routing.deviceId != "PLC");
        }

        public bool HasAnyRoutesForLocation(Location location)
        {
            return _routings
                .Any(routing => (routing.locationId == location.id ||
                                     (routing.locationId == null 
                                      && routing.locationTypeId == location.locationGroupId)))
                && _routings
                    .Any(routing => (routing.routedLocationId == location.id ||
                                     (routing.routedLocationId == null 
                                      && routing.routedLocationTypeId == location.locationGroupId)));
        }

        public bool IsRoutedBy(DeviceId deviceId, Location source, Location dest)
        {
            return _routings
                .Any(routing => routing.deviceId.Contains(deviceId.id)
                                && (routing.locationId == source.id ||
                                    (routing.locationId == null && routing.locationTypeId == source.locationGroupId))
                                && (routing.routedLocationId == dest.id ||
                                    (routing.routedLocationId == null &&
                                     routing.routedLocationTypeId == dest.locationGroupId)));
        }

        public Location GetNextLocation(Location start, Location destination)
        {
            SourceDest sourceDest;

            if (start.id == destination.id) return destination;

            sourceDest.startLocationId = start.id;
            sourceDest.destLocationId = destination.id;

            int nextLocationId = _memoryCache.GetOrCreate<int>(sourceDest, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                var route = FindRoute(start, destination);

                var routedToLocationId = route.First().routedTo.locationId;
                if (routedToLocationId != null)
                {
                    var nextLocation = (int) routedToLocationId;

                    route.ForEach(location =>
                    {
                        if (location != route.First() && location != route.Last())
                        {
                            SourceDest sourceDestNext;
                            sourceDestNext.startLocationId = (int) location.locationId;
                            sourceDestNext.destLocationId = destination.id;
                            _memoryCache.Set(sourceDestNext, location.routedTo.locationId, TimeSpan.FromHours(1));
                        }
                    });

                    return nextLocation;
                }
                throw new NextRouteLocationIsLocationGroup(start, destination, (int)route.First().routedTo.locationGroupId);
            });

            var location = _locationRepository.GetLocationById(nextLocationId);

            return location;
        }

        public List<string> GetDeviceIds(Location start, Location destination, DeviceId deviceId)
        {
            return _routings.Where(route => (route.locationId == start.id
                                             || route.locationTypeId != null &&
                                             route.locationTypeId == start.locationGroupId)
                                            && (route.routedLocationId == destination.id
                                                || route.routedLocationTypeId != null && route.routedLocationTypeId ==
                                                destination.locationGroupId)
                                            && route.deviceId.Contains(deviceId.id)).Select(route => route.deviceId)
                .ToList();
        }

        private List<RoutingLocationDescription> FindRoute(Location start, Location destination)
        {
            RoutingLocationDescription startLocationDescription = new RoutingLocationDescription
            {
                locationId = start.id,
                locationGroupId = start.locationGroupId,
                routeCost = 0
            };
            List<RoutingLocationDescription> unrouted = new List<RoutingLocationDescription>
            {
                startLocationDescription
            };
            List<RoutingLocationDescription> routed = new List<RoutingLocationDescription>();

            while (unrouted.Count != 0)
            {
                unrouted.ToList().ForEach(location =>
                {
                    if (CurrentLocationIsDestinationAndIsNotStartLocation(start, destination, location))
                    {
                        location.locationId = destination.id;
                    }

                    var routes = FindAllRoutesFromLocation(location);
                    routes.ForEach(route =>
                    {
                        if (NoShorterRouteForLocationExistsInRouted(routed, route))
                        {
                            unrouted.Add(route);
                        }
                    });

                    routed.Add(location);
                    unrouted.Remove(location);
                });
            }

            List<RoutingLocationDescription> finalRoute = new List<RoutingLocationDescription>();
            finalRoute.Add(routed.Where(location =>
                ((location.locationId != null && location.locationId == destination.id)
                 || (location.locationGroupId != null && location.locationGroupId == destination.locationGroupId
                 && location.locationId != start.id))).ToList().OrderBy(route => route.routeCost).First());

            while (finalRoute.Last() != routed.First())
            {
                var nextToRoute = routed.First(location => location == finalRoute.Last().routedFrom);
                nextToRoute.routedTo = finalRoute.Last();
                finalRoute.Add(nextToRoute);
            }

            finalRoute.Reverse();
            return finalRoute;
        }

        private static bool CurrentLocationIsDestinationAndIsNotStartLocation(Location start, Location destination, RoutingLocationDescription location)
        {
            return (location.locationId != null && location.locationId == destination.id)
                   || (location.locationGroupId != null &&
                       location.locationGroupId == destination.locationGroupId) 
                   && location.locationId != start.id;
        }

        private static bool NoShorterRouteForLocationExistsInRouted(List<RoutingLocationDescription> routed, RoutingLocationDescription route)
        {
            return !routed.Any(routedLocation =>
                ((routedLocation.locationId != null && routedLocation.locationId == route.locationId)
                 || (routedLocation.locationGroupId != null &&
                     routedLocation.locationGroupId == route.locationGroupId))
                && routedLocation.routeCost < route.routeCost);
        }

        private List<RoutingLocationDescription> FindAllRoutesFromLocation(RoutingLocationDescription location)
        {
            var routes = _routings.Where(routing =>
                (routing.locationId != null && routing.locationId == location.locationId)
                || (routing.locationTypeId != null && routing.locationTypeId == location.locationGroupId)).Select
            (routing => new RoutingLocationDescription
            {
                locationId = routing.routedLocationId,
                locationGroupId = routing.routedLocationTypeId,
                routeCost = location.routeCost + routing.routeCost,
                routedFrom = location
            }).ToList();
            return routes;
        }
        public List<int> GetShelfIds(Location start, Location destination, DeviceId deviceId)
        {
            return GetDeviceIds(start, destination, deviceId).Select(devId => Int32.Parse(devId[^1].ToString()))
                .ToList();
        }

        public bool AnyRouteForDeviceContainsTotesLocation(DeviceId deviceId, Location location)
        {
            if (location == null) return false;
            var storeDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            var craneIsOnTotesRoute = storeDbContext.routes
                .Where(route => route.deviceId.Contains(deviceId.id))
                .Any(route =>
                    (route.locationTypeId == location.locationGroupId && route.locationTypeId != null)
                    || (route.locationId == location.id && route.locationId != null)
                );

            storeDbContext.Dispose();

            return craneIsOnTotesRoute;
        }

        public int GetRouteCost(Location sourceLocation, Location location)
        {
            return FindRoute(sourceLocation, location).Last().routeCost;
        }
    }
}