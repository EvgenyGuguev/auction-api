using Domain;
using UseCases.Account;

namespace Api.Services
{
    public class AccountService
    {
        private readonly AuthTokenService _tokenService;

        public AccountService(AuthTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public AuthDto LoginResponse(User user)
        {
            return new()
            {
                Success = true,
                Token = _tokenService.CreateToken(user)
            };
        }

        public UserDto CurrentUserResponse(User user)
        {
            return new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName
            };
        }
    }
}