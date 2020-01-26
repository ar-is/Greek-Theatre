using GreekTheater.API.Core.Entities;
using GreekTheater.API.Helpers.Extension_Methods;
using GreekTheater.API.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.DbContexts
{
    public class GreekTheaterAPIContext : DbContext
    {
        public GreekTheaterAPIContext(DbContextOptions<GreekTheaterAPIContext> options) : base(options)
        {}

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<PerformanceGenre> PerformanceGenres { get; set; }
        public DbSet<Acting> Actings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.RemovePluralizingTableNameConvention();

            builder.ApplyConfiguration(new GenreConfiguration());
            builder.ApplyConfiguration(new DirectorConfiguration());
            builder.ApplyConfiguration(new ActorConfiguration());
            builder.ApplyConfiguration(new PerformanceConfiguration());
            builder.ApplyConfiguration(new PerformanceGenreConfiguration());
            builder.ApplyConfiguration(new ActingConfiguration());
        }
    }
}
