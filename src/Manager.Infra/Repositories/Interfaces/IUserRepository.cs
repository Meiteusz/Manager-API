using Manager.Domain.Entities;

namespace Manager.Infra.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string userEmail);
        Task<List<User>> SearchByEmail(string userEmail);
        Task<List<User>> SearchByName(string userName);
    }
}
