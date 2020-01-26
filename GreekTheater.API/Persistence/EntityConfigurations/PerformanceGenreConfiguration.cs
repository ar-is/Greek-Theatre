using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class PerformanceGenreConfiguration : IEntityTypeConfiguration<PerformanceGenre>
    {
        public void Configure(EntityTypeBuilder<PerformanceGenre> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigureEntityRelationships(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<PerformanceGenre> builder)
        {
            builder.HasKey(pg => new { pg.GenreId, pg.PerformanceId });
        }

        private void ConfigureEntityRelationships(EntityTypeBuilder<PerformanceGenre> builder)
        {
            builder
                .HasOne(pg => pg.Genre)
                .WithMany(g => g.PerformanceGenres)
                .HasForeignKey(pg => pg.GenreId);

            builder
                .HasOne(mg => mg.Performance)
                .WithMany(p => p.PerformanceGenres)
                .HasForeignKey(pg => pg.PerformanceId);
        }

        private void SeedData(EntityTypeBuilder<PerformanceGenre> builder)
        {
            builder.HasData(
                new PerformanceGenre { PerformanceId = 1, GenreId = 1 },
                new PerformanceGenre { PerformanceId = 1, GenreId = 2 },
                new PerformanceGenre { PerformanceId = 1, GenreId = 3 },
                new PerformanceGenre { PerformanceId = 2, GenreId = 3 },
                new PerformanceGenre { PerformanceId = 2, GenreId = 6 },
                new PerformanceGenre { PerformanceId = 3, GenreId = 2 },
                new PerformanceGenre { PerformanceId = 3, GenreId = 3 }
                );
        }    
    }
}
