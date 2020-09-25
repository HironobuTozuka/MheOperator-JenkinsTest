using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class LocationRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public LocationRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public List<Location> GetAllLocations()
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext).ToList();
        }

        public Location GetLocationByZone(Zone locationZone)
        {
            using var dbContext = GetDbContext();
            
            return SelectLocations(dbContext)
                .First(location =>
                    location.zoneId == locationZone.id);
        }

        public Location GetLocationByFunction(LocationFunction locationFunction)
        {
            return GetLocationsByFunction(locationFunction).First();
        }
        public List<Location> GetEmptyLocationsByFunction(LocationFunction locationFunction)
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext)
                .Where(loc => loc.zone.function == locationFunction &&
                              dbContext.totes.All(tote =>
                                  tote.locationId != loc.id && tote.storageLocationId != loc.id))
                .ToList();
        }

        public Location GetShuffleLocation(Location sourceLocation)
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext)
                .FirstOrDefault(loc => loc.locationGroupId == sourceLocation.locationGroupId
                                       && loc.status == LocationStatus.Enabled && loc.isBackLocation == false &&
                                       dbContext.totes.All(tote =>
                                           tote.locationId != loc.id &&
                                           tote.storageLocationId != loc.id));
        }

        public int ShuffleLocationsCount(Location location)
        {
            using var dbContext = GetDbContext();
            
            return dbContext.locations.Count(loc => loc.rack == location.rack
                                                    && dbContext.totes.All(tote =>
                                                        tote.storageLocationId != loc.id) &&
                                                    loc.storedTote == null
                                                    && loc.status == LocationStatus.Enabled  && !loc.isBackLocation);
        }

        public List<Location> GetLocationsByFunction(LocationFunction locationFunction)
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext)
                .Where(loc => loc.zone.function == locationFunction).ToList();
        }

        public Location GetLocationByPlcId(String plcId)
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext)
                .First(l => l.plcId == plcId);
        }
        
        public Location GetDownstreamLocation(Location location)
        {
            using var dbContext = GetDbContext();

            var previousLocations = dbContext.routes
                .Where(route => route.routedLocationId.Equals(location.id)).Select(routing => routing.locationId).ToList();
            if(previousLocations.Count > 1) throw new MoreThanOneDownstreamLocation() {Location = location};
            if(previousLocations.Count == 0) throw new NoDownstreamLocations() {Location = location};

            return SelectLocations(dbContext)
                .First(l => l.id.Equals(previousLocations.First()));
        }

        public Location GetLocationById(int id)
        {
            using var dbContext = GetDbContext();

            return SelectLocations(dbContext)
                .First(l => l.id == id);
        }

        public Zone GetZoneById(ZoneId id)
        {
            using var dbContext = GetDbContext();
            return dbContext.zones.FirstOrDefault(h => h.id == id.ToString());
        }

        public bool ZoneExists(ZoneId id)
        {
            using var dbContext = GetDbContext();
            return dbContext.zones.Any(h => h.id == id.Id);
        }

        public List<Zone> GetZones()
        {
            using var dbContext = GetDbContext();
            return dbContext.zones.ToList();
        }

        public int GetEmptyLocationCount(Zone zone)
        {
            using var dbContext = GetDbContext();
            return dbContext.locations
                .Where(location => location.zoneId == zone.id)
                .Where(location => dbContext.totes.All(tote => tote.storageLocation != location))
                .Count(location => dbContext.totes.All(tote => tote.location != location));
        }

        private StoreDbContext GetDbContext()
        {
            return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
        }

        private static IIncludableQueryable<Location, Zone> SelectLocations(StoreDbContext dbContext)
        {
            return dbContext.locations
                .Include(l => l.storedTote)
                .Include(l => l.locationGroup)
                .Include(l => l.zone);
        }
        
        public Location GetEmptyLocationInAnyZone(ToteType toteTypeToStore)
        {
            using var dbContext = GetDbContext();
            return SelectLocations(dbContext)
                .First(location => (location.zone.function == LocationFunction.Staging
                                   || location.zone.function == LocationFunction.Storage)
                                   && dbContext.totes.All(tote =>
                                       tote.storageLocationId != location.id) &&
                                   location.storedTote == null
                                   && location.locationHeight >=
                                   toteTypeToStore.GetToteHeightValue());
        }

        public List<Location> GetEmptyLocationsInZone(ZoneId zone)
        {
            using var dbContext = GetDbContext();
            return SelectLocations(dbContext).Where(location => location.zone.id == zone.Id
                                                                && !dbContext.totes.Any(tote =>
                                                                    tote.storageLocationId == location.id) &&
                                                                location.storedTote == null
                                                                && location.status == LocationStatus.Enabled ).ToList();
        }

        public bool IsRackDoubleDeep(Location location)
        {
            using var dbContext = GetDbContext();
            
            return dbContext.locations.Any(loc => location.rack == loc.rack && location.isBackLocation);
        }

        public Zone GetZoneByFunction(LocationFunction function)
        {
            using var dbContext = GetDbContext();
            return dbContext.zones.FirstOrDefault(h => h.function == function);
        }

        public void SetLocationStatus(Location location, LocationStatus locationStatus)
        {
            using var dbContext = GetDbContext();
            var locationEntity = dbContext.locations.Find(location.id);
            locationEntity.status = locationStatus;
            dbContext.SaveChanges();
        }

        public bool IsOccupied(Location location)
        {
            using var dbContext = GetDbContext();
            return dbContext.totes.Any(tote => tote.location.Equals(location));
        }
    }
}