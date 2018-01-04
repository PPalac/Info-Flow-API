using System.Threading.Tasks;
using InfoFlowAPI.Services.Interfaces;
using InfoFlowAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InfoFlowAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IConfiguration config;
        private IAccountService accountService;

        public AccountController(IConfiguration config, IAccountService accountService)
        {
            this.config = config;
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]RegisterUserViewModel user)
        {
            if (await accountService.RegisterUserAsync(user))
            {
                return Ok();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> CreateTokenAsync([FromBody]LoginViewModel login)
        {
            IActionResult response = Unauthorized();
            var user = await accountService.AuthenticateAsync(login);

            if (user != null)
            {
                var tokenString = accountService.BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }
    }
}


