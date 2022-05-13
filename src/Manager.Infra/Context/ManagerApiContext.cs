using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra
{
    public class ManagerApiContext : DbContext, IManagerApiContext
    {
        public DbSet<User> Users { get; private set; }

        public void SetDbSetUsers(DbSet<User> users) => Users = users;
    }
}
