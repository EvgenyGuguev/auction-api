using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentAssertions;
using Moq;
using UseCases.Account;
using UseCases.Shared;
using Xunit;

namespace Tests.UseCases.Account
{
    public class AccountControllerTests : MainAccountTest
    {
        [Fact]
        public async Task CreateNewUserFromUserManager_WithCorrectCredentials_ReturnCreatedUser()
        {
            var mockManager = ConfiguredMockUserManager(Users).Object;
            var newUser = new User { FirstName = "FirstName3", LastName = "LastName3", Email = "test3@email.com", UserName = "Ghost"};

            await mockManager.CreateAsync(newUser, "Password1234");
            
            Users.Count.Should().Be(3);
            Users.Last().FirstName.Should().BeEquivalentTo("FirstName3");
        }
        
        [Fact]
        public async Task RegisterAccountCommand_WithCorrectRequest_ReturnsExpectedResult()
        {
            MockMediator
                .Setup(m => m.Send(It.IsAny<Register.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<Result<UserDto>>());
            
            await MockMediator.Object.Send(new Register.Command());

            MockMediator.Verify(x => 
                x.Send(It.IsAny<Register.Command>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task RegisterAccountHandler_WithCorrectRequest_ReturnsRegisteredAccount()
        {
            var mockManager = ConfiguredMockUserManager(Users).Object;
            var commandRequest = new RegisterDto 
                { FirstName = "FirstName3", LastName = "LastName3", Email = "test3@email.com", Username = "UserName"};

            var sut = new Register.Handler(mockManager, TestMapper);
            var result = await sut.Handle(
                new Register.Command {Dto = commandRequest}, default);
            
            result.Should().BeOfType<Result<UserDto>>();
            result.Value.Username.Should().BeEquivalentTo("UserName");
        }
    }
}