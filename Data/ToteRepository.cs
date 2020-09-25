using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Schema;
using Common.Exceptions;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class ToteRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ToteRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Add(Tote tote)
        {
            lock(this){
                using var dbContext = GetDbContext();
                dbContext.totes.Add(tote);
                dbContext.SaveChanges();
            }
        }

        public bool IsReady(string barcode)
        {
            using var dbContext = GetDbContext();

            return dbContext.totes.Any(tote => tote.toteBarcode.Equals(barcode) && tote.status == ToteStatus.Ready);
        }

        public bool Any(string barcode)
        {
            using var dbContext = GetDbContext();
            return dbContext.totes.Any(tote => tote.toteBarcode.Equals(barcode));
        }
        
        public void Remove(Tote tote)
        {
            if (tote == null) return;
            using var dbContext = GetDbContext();
            var toteEntity = dbContext.totes.Find(tote.id);
            if(toteEntity != null)dbContext.totes.Remove(toteEntity);
            dbContext.SaveChanges();
        }

        public Tote GetToteByBarcode(string barcode)
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext).FirstOrDefault(t => t.toteBarcode.Equals(barcode));
        }
        
        public List<Tote> GetTotesWithoutLocation()
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext).Where(t => t.locationId == null).ToList();
        }
        
        public List<Tote> GetTotesWithBarcodeContaining(string barcodePart)
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext).Where(t => t.toteBarcode.Contains(barcodePart)).ToList();
        }

        public List<Tote> GetTotesNotOnStorageLocation()
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext)
                .Where(tote => tote.location!=null && 
                               (!tote.location.Equals(tote.storageLocation)
                               || tote.location.zone.function == LocationFunction.LoadingGate))
                .ToList();
        }

        public Tote GetToteOnLocation(int locationId)
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext).FirstOrDefault(t => t.locationId == locationId);
        }
        
        public Tote GetToteOnStorageLocation(Location location)
        {
            using var dbContext = GetDbContext();

            return SelectTotes(dbContext).FirstOrDefault(t => t.storageLocationId == location.id);
        }
        public void UpdateToteLocation(Tote tote, Location location)
        {
            using var dbContext = GetDbContext();

            var toteEntity = dbContext.totes.Find(tote.id);

            if(location != null) dbContext.totes
                .Where(t => t.locationId == location.id && t.toteBarcode != toteEntity.toteBarcode)
                .ForEachAsync(t => t.locationId = null).Wait();

            toteEntity.locationId = location?.id;
            toteEntity.lastLocationUpdate = DateTime.Now;
            tote.lastLocationUpdate = DateTime.Now;
            tote.locationId = location?.id;
            tote.location = location;
            

            dbContext.SaveChanges();
        }

        public void UpdateToteStatus(Tote tote, ToteStatus toteStatus)
        {
            using var dbContext = GetDbContext();
            
            var toteEntity = dbContext.totes.Find(tote.id);

            if(tote == null) throw new ToteNotFoundException(tote.toteBarcode);
            
            toteEntity.status = toteStatus;
            tote.status = toteStatus;

            dbContext.SaveChanges();
        }
        
        public void UpdateToteStorageLocation(Tote tote, Location location)
        {
            using var dbContext = GetDbContext();

            var toteEntity = dbContext.totes.Find(tote.id);

            toteEntity.storageLocationId = location.id;
            tote.storageLocationId = location.id;
            tote.storageLocation = location;

            dbContext.SaveChanges();
        }

        public ToteType GetToteType(ToteHeight toteHeight, TotePartitioning totePartitioning)
        {
            using var dbContext = GetDbContext();
            
            return dbContext.toteTypes.First(tt => tt.toteHeight == toteHeight
                                            && tt.totePartitioning == totePartitioning);
        }


        private StoreDbContext GetDbContext()
        {
            StoreDbContext dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();
            return dbContext;
        }
        
        private static IIncludableQueryable<Tote, Zone> SelectTotes(StoreDbContext dbContext)
        {
            return dbContext.totes.Include(t => t.type)
                .Include(t => t.location).ThenInclude(location => location.zone)
                .Include(t => t.type)
                .Include(t => t.storageLocation).ThenInclude(location => location.zone);
        }
    }
}