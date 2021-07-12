using System.Threading.Tasks;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Account;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AccountService _accountService;
        private readonly AuthTokenService _tokenService;

        public AccountController(AccountService accountService, AuthTokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            return HandleResult(await Mediator.Send(new Register.Command {Dto = dto}));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto dto)
        {
            var user = await Mediator.Send(new Login.Query {Dto = dto});

            if (user.Value == null) return Unauthorized();

            var refreshToken = await _tokenService.SetRefreshTokenCookie(user.Value);
            return Ok(_accountService.LoginResponse(user.Value, refreshToken.Token));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var user = await Mediator.Send(new RefreshToken.Query {Dto = dto});
            if (user.Value == null) return Unauthorized();
            if (user.Error != null) return Unauthorized();
            
            var refreshToken = await _tokenService.SetRefreshTokenCookie(user.Value);
            return Ok(_accountService.LoginResponse(user.Value, refreshToken.Token));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await Mediator.Send(new CurrentUser.Query());

            if (user.Value == null) return Unauthorized();

            return Ok(_accountService.CurrentUserResponse(user.Value));
        }
    }
}