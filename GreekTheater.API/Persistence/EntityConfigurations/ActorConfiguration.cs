using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigurePropertiesAttributes(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<Actor> builder)
        {
            builder
                .HasIndex(a => a.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }

        private void ConfigurePropertiesAttributes(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }

        private void SeedData(EntityTypeBuilder<Actor> builder)
        {
            builder.HasData(
                 new Actor
                 {
                     Id = 1,
                     Guid = Guid.Parse("7b75a444-994d-4936-96bf-9c3c0804e42d"),
                     FirstName = "Christos",
                     LastName = "Loulis",
                     DateOfBirth = new DateTime(1976, 04, 10)
                 },
                 new Actor
                 {
                     Id = 2,
                     Guid = Guid.Parse("a51f9b12-fb95-43e2-b010-733d53faf235"),
                     FirstName = "Iro",
                     LastName = "Mpezou",
                     DateOfBirth = new DateTime(1988, 12, 28)
                 },
                 new Actor
                 {
                     Id = 3,
                     Guid = Guid.Parse("4d459461-59da-473d-938e-f5c7c03d12d7"),
                     FirstName = "Minas",
                     LastName = "Chatzisavvas",
                     DateOfBirth = new DateTime(1948, 01, 28),
                     DateOfDeath = new DateTime(2015, 11, 30)
                 });
        }
    }
}
