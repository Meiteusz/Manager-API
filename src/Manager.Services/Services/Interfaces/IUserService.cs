using Manager.Services.DTO_s;

namespace Manager.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Create(UserDto userDto);
        Task<UserDto> Update(UserDto userDto);
        Task Remove(long userId);
        Task<UserDto> GetById(long userId);
        Task<List<UserDto>> GetAll();
        Task<UserDto> GetByEmail(string userEmail);
        Task<List<UserDto>> SearchByEmail(string userEmail);
        Task<List<UserDto>> SearchByName(string userName);
    }
}
