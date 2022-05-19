using AutoMapper;
using EscNet.Cryptography.Interfaces;
using FluentAssertions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO_s;
using Manager.Services.Services;
using Manager.Services.Services.Interfaces;
using Manager.Tests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Xunit;

namespace Manager.Tests
{
    [TestClass]
    public class UserServiceTests : MockConfigurationHelper
    {
        private readonly IUserService _sut;
        private readonly IMapper _mapperMock;

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRijndaelCryptography> _rijndaelCryptographyMock;

        public UserServiceTests()
        {
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._mapperMock = AutoMapperConfiguration.GetConfiguration();
            this._rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();

            _sut = new UserService(_userRepositoryMock.Object, _mapperMock, _rijndaelCryptographyMock.Object);
        }

        public IEnumerable<User> FakeUserList()
            => _mapperMock.Map<List<User>>(new List<UserDto>()
            {
                new UserDto() { Id = 1, Name = "fooRonaldo", Email = "fooRonaldo@gmail.com", Password = "1234567890"},
                new UserDto() { Id = 2, Name = "fooRicardo", Email = "fooRicardo@gmail.com", Password = "2345678910"},
                new UserDto() { Id = 3, Name = "fooRoger", Email = "fooRoger@gmail.com", Password = "3456789102"}
            });



        //[Fact(DisplayName = "Create Valid User")]
        [TestMethod]
        //[Trait("Category", "Services")]
        public async Task Create_WhenUserIsValid_ReturnsUserDto()
        {
            // Arrage
            var userInputDtoFake = new UserDto() { Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userInputFake = _mapperMock.Map<User>(userInputDtoFake);

            var userOutputDtoFake = new UserDto() { Id = 4, Name = "fooRosa", Email = "fooRosa@gmail.com", Password = "123321123321" };
            var userOutputFake = _mapperMock.Map<User>(userOutputDtoFake);

            var userFakeEmailEncrypted = "n23hçldfrgd9gd12ç31";

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>())).Returns(userFakeEmailEncrypted);
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(userOutputFake);

            // Act
            var result = await _sut.Create(userInputDtoFake);

            // Assert
            result.Should().BeEquivalentTo(userOutputDtoFake);
        }
    }
}