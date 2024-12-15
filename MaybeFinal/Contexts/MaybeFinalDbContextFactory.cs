using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MaybeFinal.Contexts;

namespace MaybeFinal
{
    public class MaybeFinalDbContextFactory : IDesignTimeDbContextFactory<MaybeFinalDbContext>
    {
        public MaybeFinalDbContext CreateDbContext(string[] args)
        {
            // Build a configuration from appsettings.json
            var optionsBuilder = new DbContextOptionsBuilder<MaybeFinalDbContext>();

            // Set up your connection string (replace this with your actual connection string from appsettings.json)
            var connectionString = "Server=MY-COMPUTER\\SQLEXPRESS;Database=MyData0312;Trusted_Connection=True;TrustServerCertificate=True;"; // You can use IConfiguration to read from appsettings.json

            // Configure the DbContext to use the correct connection string
            optionsBuilder.UseSqlServer(connectionString);

            return new MaybeFinalDbContext(optionsBuilder.Options);
        }
    }
}

