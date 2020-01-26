using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigurePropertiesAttributes(builder);
            ConfigureEntityRelationships(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<Director> builder)
        {
            builder
                .HasIndex(d => d.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }

        private void ConfigurePropertiesAttributes(EntityTypeBuilder<Director> builder)
        {
            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }

        private void SeedData(EntityTypeBuilder<Director> builder)
        {
            builder.HasData(
                 new Director
                 {
                     Id = 1,
                     Guid = Guid.Parse("7b75a444-994d-4936-96bf-9c3c0804e42d"),
                     FirstName = "Michail",
                     LastName = "Marmarinos",
                     DateOfBirth = new DateTime(1956, 05, 20)
                 },
                 new Director
                 {
                     Id = 2,
                     Guid = Guid.Parse("a51f9b12-fb95-43e2-b010-733d53faf235"),
                     FirstName = "Ioannis",
                     LastName = "Chouvardas",
                     DateOfBirth = new DateTime(1950, 12, 10)
                 },
                 new Director
                 {
                     Id = 3,
                     Guid = Guid.Parse("4d459461-59da-473d-938e-f5c7c03d12d7"),
                     FirstName = "Efi",
                     LastName = "Theodorou",
                     DateOfBirth = new DateTime(1962, 08, 12)
                 });
        }

        private void ConfigureEntityRelationships(EntityTypeBuilder<Director> builder)
        {
            builder
                .HasMany(d => d.Performances)
                .WithOne(p => p.Director)
                .HasForeignKey(p => p.DirectorId);
        }  
    }
}
