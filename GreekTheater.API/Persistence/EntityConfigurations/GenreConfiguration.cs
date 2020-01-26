using GreekTheater.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            ConfigureKeysAndIndexes(builder);
            ConfigurePropertiesAttributes(builder);
            SeedData(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<Genre> builder)
        {
            builder
                .HasIndex(g => g.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }

        private void ConfigurePropertiesAttributes(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);
        }

        private void SeedData(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(
                 new Genre
                 { 
                     Id = 1,
                     Guid = Guid.Parse("ca6a17f8-2045-4113-bbdb-3230427ae5cd"),
                     Name = "Musical" 
                 },
                 new Genre 
                 { 
                     Id = 2,
                     Guid = Guid.Parse("905d1d8f-d286-4ef2-a72e-c76353f342cb"),
                     Name = "Comedy" 
                 },
                 new Genre 
                 { 
                     Id = 3,
                     Guid = Guid.Parse("31e18a42-525d-42c2-99e6-93c6149a8d2f"),
                     Name = "Drama" 
                 },
                 new Genre 
                 { 
                     Id = 4,
                     Guid = Guid.Parse("d64e4e3c-6d5b-4818-9039-246861177572"),
                     Name = "Tragedy" },
                 new Genre 
                 {
                     Id = 5,
                     Guid = Guid.Parse("8cc8da1e-56a7-487c-9786-548271ef9586"),
                     Name = "Opera" },
                 new Genre 
                 { 
                     Id = 6,
                     Guid = Guid.Parse("61cc3a5f-cab1-4104-8acf-3a0440ebdbc8"),
                     Name = "Theatre of the Absurd‎" }
                 );
        }      
    }
}
