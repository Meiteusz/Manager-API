using AutoMapper;
using EscNet.Cryptography.Interfaces;
using EscNet.Hashers.Interfaces.Algorithms;
using FluentAssertions;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO_s;
using Manager.Services.Services;
using Manager.Services.Services.Interfaces;
using Manager.Tests.TestUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manager.Tests
{
    public class UserServiceTests : MockConfigurationHelper
    {
        private readonly IUserService _sut;
        private readonly IMapper _mapperMock;

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IArgon2IdHasher> _hasher;

        public UserServiceTests()
        {
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._mapperMock = AutoMapperConfiguration.GetConfiguration();
            this._hasher = new Mock<IArgon2IdHasher>();

            _sut = new UserService(_userRepositoryMock.Object, _mapperMock, _hasher.Object);
        }

        public IEnumerable<User> GetFakeUserList()
            => _mapperMock.Map<List<User>>(new List<UserDto>()
            {
                new UserDto() { Id = 1, Name = "fooRonaldo", Email = "fooRonaldo@gmail.com", Password = "1234567890"},
                new UserDto() { Id = 2, Name = "fooRicardo", Email = "fooRicardo@gmail.com", Password = "2345678910"},
                new UserDto() { Id = 3, Name = "fooRoger", Email = "fooRoger@gmail.com", Password = "3456789102"}
            });



        [Fact(DisplayName = "Create: Valid User")]
        public async Task Create_WhenUserIsValid_ReturnsUserDto()
        {
            // Arrage
            var userInputDtoFake = new UserDto() { Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userInputFake = _mapperMock.Map<User>(userInputDtoFake);

            var userOutputDtoFake = new UserDto() { Id = 4, Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userOutputFake = _mapperMock.Map<User>(userOutputDtoFake);

            var userFakePasswordHashed = "n23hçldfrgd9gd12ç31";

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            _hasher.Setup(x => x.Hash(It.IsAny<string>())).Returns(userFakePasswordHashed);
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(userOutputFake);


            // Act
            var result = await _sut.Create(userInputDtoFake);


            // Assert
            result.Should().BeEquivalentTo(userOutputDtoFake);
        }


        [Fact(DisplayName = "Create: Existing user email")]
        public async Task Create_WhenUserEmailAlreadyExists_ThrowDomainException()
        {
            // Arrage
            var userDtoFake = new UserDto() { Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userFake = _mapperMock.Map<User>(userDtoFake);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(userFake);


            // Act
            Func<Task> act = () => _sut.Create(userDtoFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("Já existe um usuário cadastrado com este email", exception.Message);
        }


        [Fact(DisplayName = "Create: Invalid user")]
        public async Task Create_WhenUserIsInvalid_ThrowDomainException() // Refactory
        {
            // Arrage
            var userDtoFake = new UserDto() { Name = "", Email = "32dasds1213.com@gmail", Password = "123" };
            var userFake = _mapperMock.Map<User>(userDtoFake);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);


            // Act
            Func<Task> act = () => _sut.Create(userDtoFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("Alguns campos estão inválidos, por favor corrija-os!", exception.Message);
            Assert.Contains("O nome não pode ser vazio", exception.Errors);
            Assert.Contains("O nome deve ter no mínimo 3 caracteres.", exception.Errors);
            Assert.Contains("O email informado não é válido.", exception.Errors);
            Assert.Contains("A senha deve ter no mínimo 8 caracteres", exception.Errors);
        }


        [Fact(DisplayName = "Get: Passing valid Id")]
        public async Task Get_WhenIdIsValid_ReturnsUserDto()
        {
            // Arrage
            var userIdFake = 3;
            var userFake = _mapperMock.Map<User>(new UserDto() 
            {   
                Id = 3, 
                Name = "fooRoger", 
                Email = "fooRoger@gmail.com", 
                Password = "3456789102" 
            });

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(userFake);


            // Act
            var result = await _sut.GetById(userIdFake);


            // Assert
            Assert.NotNull(result);
        }


        [Fact(DisplayName = "Get: Passing invalid Id")]
        public async Task Get_WhenIdIsInvalid_ReturnsDomainException()
        {
            // Arrage
            var userIdFake = -3;


            // Act
            Func<Task> act = () => _sut.GetById(userIdFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Contains("Não existe nenhum usuário com o Id informado! Id Informado", exception.Message);
        }


        [Fact(DisplayName = "Get: User not exists")]
        public async Task Get_WhenUserNotExists_ReturnsNull()
        {
            // Arrage
            var userIdFake = 999999;

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(value: null);


            // Act
            var result = await _sut.GetById(userIdFake);


            // Assert
            Assert.Null(result);
        }


        [Fact(DisplayName = "GetAll: User list with values")]
        public async Task GetAll_WhenUserListContainsValues_ReturnsUsersCountGreaterThanZero()
        {
            // Arrage
            var userListFake = GetFakeUserList().ToList();

            _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.GetAll();


            // Assert
            Assert.True(result.Count > 0);
        }


        [Fact(DisplayName = "GetAll: User list without values")]
        public async Task GetAll_WhenUserListNotContainsValues_ReturnsUsersCountEqualZero()
        {
            // Arrage
            var userListFake = new List<User>() { };

            _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.GetAll();


            // Assert
            Assert.True(result.Count == 0);
        }


        [Fact(DisplayName = "GetByEmail: Existing email")]
        public async Task GetByEmail_WhenEmailExists_ReturnsUserDto()
        {
            // Arrage
            var userEmailFake = "fooRonaldo@gmail.com";
            var userDtoFake = new UserDto()
            {
                Id = 2,
                Name = "fooRicardo",
                Email = "fooRicardo@gmail.com",
                Password = "2345678910"
            };

            var userFake = _mapperMock.Map<User>(userDtoFake);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(userFake);


            // Act
            var result = await _sut.GetByEmail(userEmailFake);


            // Assert
            result.Should().BeEquivalentTo(userDtoFake);
        }


        [Fact(DisplayName = "GetByEmail: Not existing email")]
        public async Task GetByEmail_WhenEmailNotExists_ReturnsNull()
        {
            // Arrage
            var userEmailFake = "notExistingEmail@gmail.com";

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);


            // Act
            var result = await _sut.GetByEmail(userEmailFake);


            // Assert
            Assert.Null(result);
        }


        [Fact(DisplayName = "SearchByEmail: Existing users with email")]
        public async Task SearchByEmail_WhenExistingUsersWithEmail_ReturnsUsersCountGreaterThanZero()
        {
            // Arrange
            var userEmailSearchedFake = "Ricardo";
            var userListFake = GetFakeUserList().Where(x => x.Email.Contains(userEmailSearchedFake))
                                                .ToList();

            _userRepositoryMock.Setup(x => x.SearchByEmail(It.IsAny<string>())).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.SearchByEmail(userEmailSearchedFake);


            // Assert
            Assert.True(result.Count > 0);
        }


        [Fact(DisplayName = "SearchByEmail: Not existing users with email passed")]
        public async Task SearchByEmail_WhenNotExistingUsersWithEmail_ReturnsUsersCountEqualZero()
        {
            // Arrange
            var userEmailSearchedFake = "NotExistigEmailOnUserListHahaha";
            var userListFake = GetFakeUserList().Where(x => x.Email.Contains(userEmailSearchedFake))
                                                .ToList();

            _userRepositoryMock.Setup(x => x.SearchByEmail(It.IsAny<string>())).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.SearchByEmail(userEmailSearchedFake);


            // Assert
            Assert.True(result.Count == 0);
        }


        [Fact(DisplayName = "SearchByName: Existing users with name")]
        public async Task SearchByName_WhenExistingUsersWithName_ReturnsUsersCountGreaterThanZero()
        {
            // Arrange
            var userNameSearchedFake = "foo";
            var userListFake = GetFakeUserList().Where(x => x.Name.Contains(userNameSearchedFake))
                                                .ToList();

            _userRepositoryMock.Setup(x => x.SearchByName(It.IsAny<string>())).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.SearchByName(userNameSearchedFake);


            // Assert
            Assert.True(result.Count > 0);
        }


        [Fact(DisplayName = "SearchByName: Not existing users with name")]
        public async Task SearchByName_WhenNotExistingUsersWithName_ReturnsUsersCountEqualZero()
        {
            // Arrange
            var userNameSearchedFake = "NotExistigNameOnUserListHahaha";
            var userListFake = GetFakeUserList().Where(x => x.Name.Contains(userNameSearchedFake))
                                                .ToList();

            _userRepositoryMock.Setup(x => x.SearchByName(It.IsAny<string>())).ReturnsAsync(userListFake);


            // Act
            var result = await _sut.SearchByName(userNameSearchedFake);


            // Assert
            Assert.True(result.Count == 0);
        }


        [Fact(DisplayName = "Update: Valid user")]
        public async Task Update_WhenUserIsValid_ReturnsUserDto()
        {
            // Arrange
            var userDtoFake = new UserDto() 
            { 
                Id = 1, 
                Name = "fooRonaldo", 
                Email = "fooRonaldo@gmail.com", 
                Password = "1234567890" 
            };

            var userFake = _mapperMock.Map<User>(userDtoFake);

            var userDtoUpdatedFake = new UserDto()
            {
                Id = 1,
                Name = "fooRonaldoAtualizado",
                Email = "fooRonaldoAtualizado@gmail.com",
                Password = "1234567890"
            };

            var userUpdatedFake = _mapperMock.Map<User>(userDtoUpdatedFake);

            var userFakePasswordHash = "n23hçldfrgd9gd12ç31";

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(userFake);
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            _hasher.Setup(x => x.Hash(It.IsAny<string>())).Returns(userFakePasswordHash);
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).ReturnsAsync(userUpdatedFake);


            // Act
            var result = await _sut.Update(userDtoFake);


            // Assert
            result.Should().BeEquivalentTo(userDtoUpdatedFake);
        }


        [Fact(DisplayName = "Update: Invalid Id")]
        public async Task Update_WhenUserIdIsInvalid_ReturnsDomainException()
        {
            // Arrange
            var userDtoFake = new UserDto()
            {
                Id = 99999999,
                Name = "fooRonaldo",
                Email = "fooRonaldo@gmail.com",
                Password = "1234567890"
            };

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(value: null);


            // Act
            Func<Task> act = () => _sut.Update(userDtoFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Contains("Não existe nenhum usuário com o Id informado! Id Informado", exception.Message);
        }


        [Fact(DisplayName = "Update: Existing user email")]
        public async Task Update_WhenUserEmailAlreadyExists_ThrowDomainException()
        {
            // Arrage
            var userDtoFake = new UserDto() { Id = 4, Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userFake = _mapperMock.Map<User>(userDtoFake);

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(userFake);
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(userFake);


            // Act
            Func<Task> act = () => _sut.Update(userDtoFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("Já existe um usuário cadastrado com este email", exception.Message);
        }


        [Fact(DisplayName = "Update: Invalid user")]
        public async Task Update_WhenUserIsInvalid_ThrowDomainException() // Refactory
        {
            // Arrage
            var userDtoFake = new UserDto() { Id = 4, Name = "", Email = "abcdefg@gmail.com", Password = "123456789" };
            var userFake = _mapperMock.Map<User>(userDtoFake);

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(userFake);
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);


            // Act
            Func<Task> act = () => _sut.Create(userDtoFake);


            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("Alguns campos estão inválidos, por favor corrija-os!", exception.Message);
            Assert.Contains("O nome não pode ser vazio", exception.Errors);
            Assert.Contains("O nome deve ter no mínimo 3 caracteres.", exception.Errors);
        }


        [Fact(DisplayName = "Remove: Invalid Id")]
        public async Task Remove_WhenUserIdIsInvalid_ThrowDomainException()
        {
            // Arrange
            var userIdFake = -3;

            // Act
            Func<Task> act = () => _sut.Remove(userIdFake);

            // Assert
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Contains("Não existe nenhum usuário com o Id informado! Id Informado", exception.Message);
        }
    }
}