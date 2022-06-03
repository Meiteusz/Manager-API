using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        private readonly IManagerApiContext _context;

        public BaseRepository(IManagerApiContext context)
        {
            this._context = context;
        }

        public virtual async Task<T> Create(T obj)
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public virtual async Task Delete(long id)
        {
            var obj = await Get(id);

            if (obj != null)
            {
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<T?> Get(long id)
            => await _context.Set<T>()
                             .AsNoTracking()
                             .Where(x => x.Id == id)
                             .FirstOrDefaultAsync();

        public virtual async Task<List<T>> GetAll()
            => await _context.Set<T>()
                             .AsNoTracking()
                             .ToListAsync();

        public virtual async Task<T> Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return obj;
        }
    }
}
