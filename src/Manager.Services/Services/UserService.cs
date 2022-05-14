using AutoMapper;
using Manager.Core.Exceptions;
using Manager.Core.BusinessHelpers;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO_s;
using Manager.Services.Services.Interfaces;

namespace Manager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<UserDto> Create(UserDto userDto)
        {
            var userExists = await _userRepository.GetByEmail(userDto.Email) != null;

            if (userExists)
                throw new DomainException("Já existe um usuário cadastrado com este email");

            var user = _mapper.Map<User>(userDto);
            user.Validate();

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDto>(userCreated);
        }

        public async Task<UserDto> GetById(long userId)
        {
            if (userId.IsInvalidID())
                DomainException.ThrowDomainExceptionInvalidId(userId);

            var user = await _userRepository.Get(userId);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var usersList = await _userRepository.GetAll();

            return _mapper.Map<List<UserDto>>(usersList);
        }

        public async Task<UserDto> GetByEmail(string userEmail)
        {
            var user = await _userRepository.GetByEmail(userEmail);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> SearchByEmail(string userEmail)
        {
            var userList = await _userRepository.SearchByEmail(userEmail);

            return _mapper.Map<List<UserDto>>(userList);
        }

        public async Task<List<UserDto>> SearchByName(string userName)
        {
            var userList = await _userRepository.SearchByName(userName);

            return _mapper.Map<List<UserDto>>(userList);
        }

        public async Task<UserDto> Update(UserDto userDto)
        {
            var userExists = _userRepository.Get(userDto.Id) != null;

            if (!userExists)
                DomainException.ThrowDomainExceptionInvalidId(userDto.Id);

            var user = _mapper.Map<User>(userDto);
            user.Validate();

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDto>(userUpdated);
        }

        public async Task Remove(long userId)
        {
            if (userId.IsInvalidID())
                DomainException.ThrowDomainExceptionInvalidId(userId);

            await _userRepository.Delete(userId);
        }
    }
}
