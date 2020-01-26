using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class ActingConfiguration : IEntityTypeConfiguration<Acting>
    {
        public void Configure(EntityTypeBuilder<Acting> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigurePropertiesAttributes(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<Acting> builder)
        {
            builder
                .HasIndex(g => g.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }

        private void ConfigurePropertiesAttributes(EntityTypeBuilder<Acting> builder)
        {
            builder.Property(a => a.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        }

        private void SeedData(EntityTypeBuilder<Acting> builder)
        {
            builder.HasData(
                 new Acting
                 {
                     Id = 1,
                     Guid = Guid.Parse("a9eb16c3-d4d6-4280-89ad-8e1bba35be38"),
                     RoleName = "Don Alfonso",
                     ActorId = 1,
                     PerformanceId = 1
                 },
                 new Acting
                 {
                     Id = 2,
                     Guid = Guid.Parse("8b1877a9-1896-4b7b-a1c4-97957a919958"),
                     RoleName = "Don Juan",
                     ActorId = 3,
                     PerformanceId = 1
                 },
                 new Acting
                 {
                     Id = 3,
                     Guid = Guid.Parse("ef8fd6fb-0f54-4247-b2c5-dcc851034b1d"),
                     RoleName = "Vladimir",
                     ActorId = 1,
                     PerformanceId = 2
                 },
                 new Acting
                 {
                     Id = 4,
                     Guid = Guid.Parse("344ac739-b7ff-4b50-916b-77882688be7c"),
                     RoleName = "Estragon",
                     ActorId = 2,
                     PerformanceId = 2
                 },
                 new Acting
                 {
                     Id = 5,
                     Guid = Guid.Parse("8a113a93-2051-490d-9329-bad2920e9b97"),
                     RoleName = "McLeavy",
                     ActorId = 1,
                     PerformanceId = 3
                 },
                 new Acting
                 {
                     Id = 6,
                     Guid = Guid.Parse("5c4a34ac-be45-4419-8a8b-612c020b38cf"),
                     RoleName = "Fay",
                     ActorId = 2,
                     PerformanceId = 3
                 }
                 );
        }
    }
}
