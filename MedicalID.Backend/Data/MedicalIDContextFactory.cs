using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MedicalID.Backend.Data
{
    public class MedicalIDContextFactory : IDesignTimeDbContextFactory<MedicalIDContext>
    {
        public MedicalIDContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MedicalIDContext>();
            var connectionString = configuration.GetConnectionString("MedicalIDContext");

            optionsBuilder.UseSqlServer(connectionString);

            return new MedicalIDContext(optionsBuilder.Options);
        }
    }
}

