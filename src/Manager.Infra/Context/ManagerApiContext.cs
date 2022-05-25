using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Manager.Infra
{
    public class ManagerApiContext : DbContext, IManagerApiContext
    {
        private readonly IConfiguration _configuration;

        public ManagerApiContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public DbSet<User> Users { get; private set; }

        public void SetDbSetUsers(DbSet<User> users) => Users = users;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:ManagerAPIAzureConnection"]);
    }
}
