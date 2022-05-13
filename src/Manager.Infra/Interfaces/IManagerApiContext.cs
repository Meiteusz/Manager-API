using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Interfaces
{
    public interface IManagerApiContext
    {
        DbSet<User> Users { get; }
        void SetDbSetUsers(DbSet<User> users);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
