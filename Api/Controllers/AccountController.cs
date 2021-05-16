using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UseCases.Account;

namespace Api.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            return HandleResult(await Mediator.Send(new Register.Command {Dto = dto}));
        }
    }
}