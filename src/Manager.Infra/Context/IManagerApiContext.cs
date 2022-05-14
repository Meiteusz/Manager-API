using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Manager.Infra.Interfaces
{
    public interface IManagerApiContext
    {
        DbSet<User> Users { get; }
        void SetDbSetUsers(DbSet<User> users);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry Remove(object entity);
        EntityEntry Entry(object entity);
    }
}
