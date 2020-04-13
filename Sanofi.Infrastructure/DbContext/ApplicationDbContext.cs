using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sanofi.Core;
using Sanofi.Core.EntitiesModel.Administrator;
using Sanofi.Core.EntitiesModel.IdentityCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanofi.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public DbSet<Feature> Feature { get; set; }
        public DbSet<RoleFeature> RoleFeature { get; set; }

        public static string ConString = AppSettingJson.GetConnectionString();

        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(ConString);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: base(options)
        {

        }

        public static ApplicationDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(ConString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(ConString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
