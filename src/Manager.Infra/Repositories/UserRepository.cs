using Manager.Domain.Entities;
using Manager.Infra.Interfaces;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ManagerApiContext _context;

        public UserRepository(ManagerApiContext context) : base(context)
        {
            this._context = context;
        }

        public Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> SearchByEmail()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> SearchByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
