using System;
using System.Linq;
using Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Common.Exceptions;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using RcsLogic.Models.Device;

namespace RcsLogic.Services
{
    public class LocationService
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private List<Location> pickLocationsRackA;
        private Location pickLocationRackB;
        private Location placeLocation;
        private readonly RoutingService _routingService;


        public LocationService(ILoggerFactory loggerFactory,
            LocationRepository locationRepository,
            ToteRepository toteRepository,
            RoutingService routingService)
        {
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _routingService = routingService;
            _logger = loggerFactory.CreateLogger<LocationService>();
            DisableLocationsWithoutSourceDestRoute();
        }

        private void DisableLocationsWithoutSourceDestRoute()
        {
            _locationRepository.GetAllLocations()
                .ForEach(location =>
                {
                    if(!_routingService.HasAnyRoutesForLocation(location))
                    {
                        _logger.LogInformation($"Location {location} will be disabled, since no route is present");
                        if(location.status == Common.Models.Location.LocationStatus.Enabled)
                            _locationRepository.SetLocationStatus(location,
                            Common.Models.Location.LocationStatus.NotAccessible);
                        
                    }
                    else if(location.status == Common.Models.Location.LocationStatus.NotAccessible)
                    {
                        _logger.LogInformation($"Location {location} will be enabled, since routes are now present");
                        _locationRepository.SetLocationStatus(location,
                            Common.Models.Location.LocationStatus.Enabled);
                    }
                });
        }

        public void AssignDestLocationsToTaskBundle(TaskBundle taskBundle)
        {
            UpdatePickAndPlaceLocations();
            AssignPickTaskDestLocations(taskBundle);
            AssignMoveTaskDestLocations(taskBundle);
        }

        private void AssignMoveTaskDestLocations(TaskBundle taskBundle)
        {
            taskBundle.tasks.OfType<MoveTask>().ToList().ForEach(task =>
            {
                var tote = _toteRepository.GetToteByBarcode(task.toteId);
                task.destLocation = _FindLocation(task.destZone, tote);
                _logger.LogDebug("Locations assigned for move task: Tote: {0}, {1}",
                    task.toteId, task.destLocation.plcId);
            });
        }

        private void AssignPickTaskDestLocations(TaskBundle taskBundle)
        {
            int i = 0;

            List<KeyValuePair<string, Location>> assignedLocations = new List<KeyValuePair<string, Location>>();

            taskBundle.tasks.OfType<PickTask>().ToList().ForEach(task =>
            {
                task.destTote.pickLocation = placeLocation;
                //If tote is from racking A
                var tote = _toteRepository.GetToteByBarcode(task.sourceTote.toteId);
                if (ToteShouldBeAssignedToRackAPickLocations(tote))
                {
                    if (ToteIsNotAssignedToRackAPickLocation(task))
                    {
                        if (!assignedLocations.Any(tote =>
                            string.Equals(tote.Key, task.sourceTote.toteId, StringComparison.Ordinal)))
                        {
                            task.sourceTote.pickLocation =
                                i % 2 == 0 ? pickLocationsRackA.Last() : pickLocationsRackA.First();
                            assignedLocations.Add(new KeyValuePair<string, Location>(task.sourceTote.toteId,
                                task.sourceTote.pickLocation));
                            i++;
                        }
                        else
                        {
                            task.sourceTote.pickLocation =
                                assignedLocations.First(tote => tote.Key == task.sourceTote.toteId).Value;
                        }
                    }
                }
                else
                {
                    task.sourceTote.pickLocation = pickLocationRackB;
                }

                _logger.LogInformation("Locations assigned for pick task: SourceTote: {0}, {1}, DestTote: {2}, {3}",
                    task.sourceTote.toteId, task.sourceTote.pickLocation.plcId, task.destTote.toteId,
                    task.destTote.pickLocation.plcId);
            });
        }

        private bool ToteIsNotAssignedToRackAPickLocation(PickTask task)
        {
            return task.sourceTote.pickLocation == null ||
                   pickLocationsRackA.All(location => location.id != task.sourceTote.pickLocation.id);
        }

        private void UpdatePickAndPlaceLocations()
        {
            //Find pick and place locations
            var pickLocations = _locationRepository.GetLocationsByFunction(LocationFunction.Pick);

            pickLocationsRackA = pickLocations.Where(location => location.row == 2).ToList();
            pickLocationRackB = pickLocations.First(location => location.row == 3);
            placeLocation = _locationRepository.GetLocationsByFunction(LocationFunction.Place).First();
        }

        private static bool ToteShouldBeAssignedToRackAPickLocations(Tote tote)
        {
            return (tote.location != null &&
                    ((tote.location.locationGroupId != null && tote.location.locationGroupId <= 3)
                     || tote.location.zone.function == LocationFunction.Conveyor ||
                     tote.location.plcId.Contains("CA_P")
                     || tote.location.zone.function == LocationFunction.LoadingGate))
                   ||
                   (tote.storageLocation != null && tote.storageLocation.locationGroupId <= 3);
        }

//        private Location _FindLocation(ZoneId zone, Tote tote)
        public Location _FindLocation(ZoneId zone, Tote tote)
        {
            var zoneEntity = _locationRepository.GetZoneById(zone);
            switch (zoneEntity.function)
            {
                case LocationFunction.Staging:
                case LocationFunction.Storage:
                    _AssignToteTo(zone, tote);
                    return tote.storageLocation;
                case LocationFunction.Place:
                {
                    var storageZoneEntity = _locationRepository.GetZoneByFunction(LocationFunction.Staging);
                    _AssignToteTo(new ZoneId(storageZoneEntity.id), tote);
                    break;
                }
            }

            return _locationRepository.GetLocationByZone(zoneEntity);
            
        }

        public void _AssignToteTo(ZoneId zone, Tote tote)
        {
            if (_IsToteAssignedTo(zone, tote))
            {
                _logger.LogTrace("Storage location: {0} already assigned for Tote: {1} in zone: {2}",
                     tote.storageLocation, tote, zone);
                return;
            }

            var location = GetToteStorageLocationFromZone(zone, tote);

            if (location == null) throw new NoLocationForToteInZoneException(tote, zone);

            _toteRepository.UpdateToteStorageLocation(tote, location);
            
            _logger.LogTrace("Storage location: {0} assigned for Tote: {1} in zone: {2}",
                     tote.storageLocation, tote, zone);
        }

        public Location GetToteStorageLocationFromZone(ZoneId zone, Tote tote)
        {
            var locations = _locationRepository.GetEmptyLocationsInZone(zone);
            var location = (locations.FirstOrDefault(location =>  location.locationHeight ==
                                                                           tote.type.GetToteHeightValue() &&
                                                                           location.isBackLocation) ??
                            locations.FirstOrDefault(location => location.locationHeight ==
                                                                           tote.type.GetToteHeightValue())) ??
                           locations.FirstOrDefault(location =>  location.locationHeight >=
                                                                          tote.type.GetToteHeightValue());

            if (location != null && (!_locationRepository.IsRackDoubleDeep(location) ||
                                     location.isBackLocation != false ||
                                     _locationRepository.ShuffleLocationsCount(location) > 2)) return location;
            
            _logger.LogWarning($"Location not found for tote {tote.toteBarcode} in zone {zone}");
            return null;

        }


        private bool _IsToteAssignedTo(ZoneId zone, Tote tote)
        {
            return tote.storageLocation.zone.id.Equals(zone.Id);
        }

        public List<KeyValuePair<Zone, int>> GetZonesEmptyLocationCounts()
        {
            return _locationRepository.GetZones()
                .Select(zone =>
                    new KeyValuePair<Zone, int>(zone, _locationRepository.GetEmptyLocationCount(zone)))
                .ToList();
        }

        public Location GetClosestLocationOfFunction(Location sourceLocation, LocationFunction destLocationFunction)
        {
            return _locationRepository.GetLocationsByFunction(destLocationFunction)
                .Select(location => new KeyValuePair<Location,int>(location, _routingService.GetRouteCost(sourceLocation, location)))
                .OrderBy(location => location.Value)
                .First().Key;
            
        }
    }
}