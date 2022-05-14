using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IManagerApiContext _context;

        public UserRepository(IManagerApiContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<User?> GetByEmail(string userEmail)
            => await _context.Users
                             .Where(x => x.Email.ToLower() == userEmail.ToLower())
                             .AsNoTracking()
                             .FirstOrDefaultAsync();

        public async Task<List<User>> SearchByEmail(string userEmail)
            => await _context.Users
                             .Where(x => x.Email.ToLower().Contains(userEmail.ToLower()))
                             .AsNoTracking()
                             .ToListAsync();

        public async Task<List<User>> SearchByName(string userName)
            => await _context.Users
                             .Where(x => x.Name.ToLower().Contains(userName.ToLower()))
                             .AsNoTracking()
                             .ToListAsync();
    }
}
