using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class PerformanceConfiguration : IEntityTypeConfiguration<Performance>
    {
        public void Configure(EntityTypeBuilder<Performance> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigurePropertiesAttributes(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<Performance> builder)
        {
            builder
                .HasIndex(p => p.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }

        private void ConfigurePropertiesAttributes(EntityTypeBuilder<Performance> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void SeedData(EntityTypeBuilder<Performance> builder)
        {
            builder.HasData(
                 new Performance
                 {
                     Id = 1,
                     Guid = Guid.Parse("7b75a444-994d-4936-96bf-9c3c0804e42d"),
                     Title = "Don Juan",
                     PremiereDate = new DateTime(1996, 04, 10),
                     DirectorId = 2
                 },
                 new Performance
                 {
                     Id = 2,
                     Guid = Guid.Parse("a51f9b12-fb95-43e2-b010-733d53faf235"),
                     Title = "Waiting for Godot",
                     PremiereDate = new DateTime(2017, 04, 10),
                     DirectorId = 1
                 },
                 new Performance
                 {
                     Id = 3,
                     Guid = Guid.Parse("4d459461-59da-473d-938e-f5c7c03d12d7"),
                     Title = "Loot",
                     DirectorId = 3
                 });
        }
    }
}
