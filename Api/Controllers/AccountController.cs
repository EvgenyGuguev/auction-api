using System.Threading.Tasks;
using Api.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Account;

namespace Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {
        private readonly AuthTokenService _tokenService;

        public AccountController(AuthTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        // [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            return HandleResult(await Mediator.Send(new Register.Command {Dto = dto}));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto dto)
        {
            var loginQueryResult = await Mediator.Send(new Login.Query {Dto = dto});

            if (loginQueryResult.Value == null) return Unauthorized();

            return Ok(CreateUserResponse(loginQueryResult.Value));
        }

        private UserDto CreateUserResponse(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}