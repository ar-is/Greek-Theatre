using System;
using System.Collections.Generic;
using System.Text;
using GreekTheater.WebApp.Core.Entities;
using GreekTheater.WebApp.Persistence.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreekTheater.WebApp.Persistence.DbContexts
{
    public class TheaterWebAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public TheaterWebAppDbContext(DbContextOptions<TheaterWebAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }
}
