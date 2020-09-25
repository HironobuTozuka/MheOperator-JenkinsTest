using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Common.Models.Location;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Models
{
    public static class LocationExtention
    {
        public static List<Location> OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Location>().ToTable("Locations", "mhe");
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.plcId).IsUnique();
                entity.Property(e => e.locationHeight).HasDefaultValue(0);
                entity.Property(e => e.isBackLocation).HasDefaultValue(false);
                entity.HasOne(e => e.locationGroup).WithMany().HasForeignKey(e => e.locationGroupId)
                    .HasConstraintName("ForeignKey_Location_LocationGroup").IsRequired(false);
                entity.HasOne(e => e.frontLocation).WithMany().HasForeignKey(e => e.frontLocationId)
                    .HasConstraintName("ForeignKey_Location_FrontLocation").IsRequired(false);
                entity.Property(e => e.rack);
                entity.Property(e => e.col);
                entity.Property(e => e.row);
                entity.Property(e => e.status).IsRequired(true).HasConversion(new EnumToStringConverter<LocationStatus>()).HasDefaultValue(LocationStatus.Enabled);
                entity.HasOne(e => e.zone).WithMany().HasForeignKey(e => e.zoneId);
            });
            int index = 1;
            var locations = new List<Location>();
            locations.AddRange(Seed.Cranes.Seed(ref index));
            locations.AddRange(Seed.ConveyorLocations.Seed(ref index));
            locations.AddRange(Seed.LoadingGate.Seed(ref index));
            locations.AddRange(Seed.OrderGates.Seed(ref index));
            locations.AddRange(Seed.RackALocations.Seed(ref index));
            locations.AddRange(Seed.RackBLocations.Seed(ref index));
            modelBuilder.Entity<Location>().HasData(locations);
            return locations;
        }
    }

    public static class LocationGroupExtention
    {
        public static List<LocationGroup> OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<LocationGroup>().ToTable("LocationGroups", "mhe");
            modelBuilder.Entity<LocationGroup>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });
            var locationTypes = Seed.LocationGroups.Seed();
            modelBuilder.Entity<LocationGroup>().HasData(locationTypes);
            return locationTypes;
        }
    }

    public static class LocationZoneExtention
    {
        public static List<Zone> OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Zone>().ToTable("Zones", "mhe");
            modelBuilder.Entity<Zone>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.function).HasConversion(new EnumToStringConverter<LocationFunction>());
                entity.Property(e => e.temperatureRegime).HasConversion(new EnumToStringConverter<TemperatureRegime>());
                entity.Property(e => e.plcGateId).IsRequired(false);
            });
            var zones = Seed.LocationZones.Seed();
            modelBuilder.Entity<Zone>().HasData(zones);
            return zones;
        }
    }

    public static class RouteExtention
    {
        public static List<Route> OnModelCreating(ModelBuilder modelBuilder, List<Location> locations, List<LocationGroup> locationGroups)
        {
            // Map table names
            modelBuilder.Entity<Route>().ToTable("Routes", "mhe");
            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.deviceId).IsRequired();
                entity.HasOne(e => e.location).WithMany().HasForeignKey(e => e.locationId)
                    .HasConstraintName("ForeignKey_Location").IsRequired(false);
                entity.HasOne(e => e.routedLocation).WithMany().HasForeignKey(e => e.routedLocationId)
                    .HasConstraintName("ForeignKey_RoutedLocation").IsRequired(false);
                entity.HasOne(e => e.LocationGroup).WithMany().HasForeignKey(e => e.locationTypeId)
                    .HasConstraintName("ForeignKey_LocationType").IsRequired(false);
                entity.HasOne(e => e.RoutedLocationGroup).WithMany().HasForeignKey(e => e.routedLocationTypeId)
                    .HasConstraintName("ForeignKey_RoutedLocationType").IsRequired(false);
                entity.Property(e => e.isDefaultRoute);
                entity.Property(e => e.routeCost);
                entity.HasCheckConstraint("Constraint_OneSourceNotNull",
                    "NOT ((\"Routes\".\"locationId\" IS NULL) AND (\"Routes\".\"locationTypeId\" IS NULL))");
                entity.HasCheckConstraint("Constraint_OneDestNotNull",
                    "NOT ((\"Routes\".\"routedLocationId\" IS NULL) AND (\"Routes\".\"routedLocationTypeId\" IS NULL))");
            });

            var routes = Seed.Routes.Seed(locations, locationGroups);
            modelBuilder.Entity<Route>().HasData(routes);
            return routes;
        }
    }
}