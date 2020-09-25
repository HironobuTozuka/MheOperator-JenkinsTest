using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Models.Location;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;

namespace RcsLogic
{
    public class ServicedLocationProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ServicedLocationProvider> _logger;

        public ServicedLocationProvider(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<ServicedLocationProvider>();
        }

        public List<ServicedLocation> GetOrderGateServicedLocations()
        {
            StoreDbContext dbContext =
                _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            List<ServicedLocation> servicedLocations = dbContext.locations
                .Where(location => location.zone.function == LocationFunction.OrderGate)
                .Include(location => location.locationGroup)
                .Select(location => new ServicedLocation(location)).ToList();

            dbContext.Dispose();

            return servicedLocations;
        }
        
        public List<ServicedLocation> GetRobotServicedLocations()
        {
            StoreDbContext dbContext =
                _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

            List<ServicedLocation> servicedLocations = dbContext.locations
                .Where(location => location.zone.function == LocationFunction.Pick || location.zone.function == LocationFunction.Place)
                .Include(location => location.locationGroup)
                .Select(location => new ServicedLocation(location)).ToList();

            dbContext.Dispose();

            return servicedLocations;
        }

        public List<ServicedLocation> GetServicedLocations(DeviceId deviceId)
        {
            var storeDbContext = GetDbContext();
            List<ServicedLocation> servicedLocations = storeDbContext.locations
                .Include(l => l.locationGroup)
                .Include(l => l.zone)
                .Where(location => storeDbContext.routes.Any(route =>
                    route.deviceId.Contains(deviceId.id) &&
                    (Equals(route.locationId, location.id) || Equals(route.routedLocationId, location.id))))
                .Include(location => location.locationGroup).Distinct()
                .Select(location => new ServicedLocation
                (
                    location
                )).ToList();
            servicedLocations.ForEach(location =>
                _logger.LogInformation("Serviced location {0} added for crane {1}", location.PlcId, deviceId)
            );
            storeDbContext.Dispose();
            return servicedLocations;
        }

        private StoreDbContext GetDbContext()
        {
            StoreDbContext storeDbContext =
                _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
            return storeDbContext;
        }
    }
}