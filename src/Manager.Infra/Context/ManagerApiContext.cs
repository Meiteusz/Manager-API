using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Manager.Infra
{
    public class ManagerApiContext : DbContext, IManagerApiContext
    {
        public DbSet<User> Users { get; private set; }

        public void SetDbSetUsers(DbSet<User> users) => Users = users;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                      .AddJsonFile("appsettings.json")
                      .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ManagerAPIDefaultConnection"));
        }
    }
}
