using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;

namespace Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Location> locations { get; set; }
        public DbSet<LocationGroup> locationGroups { get; set; }
        public DbSet<Zone> zones { get; set; }
        public DbSet<Tote> totes { get; set; }
        public DbSet<ToteType> toteTypes { get; set; }
        public DbSet<Route> routes { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("mhe");
            var locationGroups = LocationGroupExtention.OnModelCreating(modelBuilder);
            LocationZoneExtention.OnModelCreating(modelBuilder);
            var locations = LocationExtention.OnModelCreating(modelBuilder);
            ToteTypeExtention.OnModelCreating(modelBuilder);
            ToteExtention.OnModelCreating(modelBuilder);
            RouteExtention.OnModelCreating(modelBuilder, locations, locationGroups);


            base.OnModelCreating(modelBuilder);
        }

        public void Initialize()
        {
            Database.Migrate();
        }
    }
}