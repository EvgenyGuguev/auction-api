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

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
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

            return Ok(_accountService.LoginResponse(user.Value));
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