using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Common.Models.Tote;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Models
{
    public class ToteExtention
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Tote>().ToTable("Totes", "mhe");
            modelBuilder.Entity<Tote>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.toteBarcode).IsUnique();
                entity.HasOne(e => e.location).WithOne(e => (Tote) e.storedTote).HasForeignKey<Tote>(e => e.locationId)
                    .HasConstraintName("ForeignKey_Tote_Location");
                entity.HasOne(e => e.type).WithMany().HasForeignKey(e => e.typeId)
                    .HasConstraintName("ForeignKey_Tote_ToteType");
                entity.Property(e => e.toteBarcode);
                entity.Property(e => e.lastLocationUpdate).HasDefaultValueSql("NOW()");
                entity.Property(e => e.status).HasDefaultValue(ToteStatus.Ready);
            });
        }
    }

    public class ToteTypeExtention
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<ToteType>().ToTable("ToteTypes", "mhe");
            modelBuilder.Entity<ToteType>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.totePartitioning).HasConversion(new EnumToStringConverter<TotePartitioning>());
                entity.Property(e => e.toteHeight).HasConversion(new EnumToStringConverter<ToteHeight>());
            });
            modelBuilder.Entity<ToteType>().HasData(Seed.ToteTypes.Seed());
        }
    }
}