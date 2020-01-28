using GreekTheater.WebApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.WebApp.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            ConfigureKeysAndIndexes(builder);
        }

        private void ConfigureKeysAndIndexes(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasIndex(u => u.Guid)
                .HasName("Guid")
                .IsClustered(false);
        }
    }
}
