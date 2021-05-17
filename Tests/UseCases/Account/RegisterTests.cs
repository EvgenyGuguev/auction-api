using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Moq;
using UseCases.Account;
using UseCases.Shared;
using Xunit;

namespace Tests.UseCases.Account
{
    public class AccountControllerTests : MainAccountTest
    {
        [Fact]
        public async Task CreateNewUserWithUserManager_WithCorrectCredentials_ReturnNewUser()
        {
            var mockManager = MockUserManager(Users).Object;
            var newUser = new User { FirstName = "FirstName3", LastName = "LastName3", Email = "test3@email.com", UserName = "Ghost"};

            await mockManager.CreateAsync(newUser, "Password1234");
            
            Assert.Equal(3, Users.Count);
            Assert.Equal("FirstName3", Users.Last().FirstName);
        }
        
        [Fact]
        public async Task RegisterMediatorCommandTest()
        {
            Mediator
                .Setup(m => m.Send(It.IsAny<Register.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<Result<UserDto>>());
            
            await Mediator.Object.Send(new Register.Command());

            Mediator.Verify(x => 
                x.Send(It.IsAny<Register.Command>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task RegisterAccountHandler_WithCorrectRequest_ReturnCorrectNewAccount()
        {
            var mockManager = MockUserManager(Users).Object;
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            
            var commandRequest = new RegisterDto 
                { FirstName = "FirstName3", LastName = "LastName3", Email = "test3@email.com", Username = "Ghost"};

            var sut = new Register.Handler(mockManager, mapper);
            var result = await sut.Handle(
                new Register.Command {Dto = commandRequest}, default);

            Assert.NotNull(result);
            Assert.IsType<Result<UserDto>>(result);
            Assert.IsType<UserDto>(result.Value);
            Assert.Equal("Ghost", result.Value.Username);
        }
    }
}