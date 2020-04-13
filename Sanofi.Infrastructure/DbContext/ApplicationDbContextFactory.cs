using Microsoft.EntityFrameworkCore;
using Sanofi.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanofi.Infrastructure.DbContext
{
    public class ApplicationDbContextFactory
    {
        private static readonly string ConnectionString = AppSettingJson.GetConnectionString();

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new ApplicationDbContext(optionsBuilder.Options);

        }
    }
}