using System.Threading.Tasks;
using InfoFlow.API.Services.Interfaces;
using InfoFlow.API.ViewModels;
using InfoFlow.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InfoFlow.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IConfiguration config;
        private IAccountService accountService;
        private UserManager<User> userManager;
        private IEmailSenderService emailSender;

        public AccountController(IConfiguration config, IAccountService accountService, UserManager<User> userManager, IEmailSenderService emailSender)
        {
            this.config = config;
            this.accountService = accountService;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterStudent")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]RegisterUserViewModel user)
        {
            var isParamValid = accountService.CheckRegisterToken(user.RegisterToken);

            if (isParamValid)
            {
                var result = await accountService.RegisterStudentAsync(user);

                if (result == bool.TrueString)
                {
                    var student = await userManager.FindByNameAsync(user.Username);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(student);
                    var callbackUrl = Url.Action("Confirm", "Account", new { userId = student.Id, token = code }, HttpContext.Request.Scheme);
                    await emailSender.SendEmail(user.Email, "Confirm", $"<a href='{callbackUrl}'>Confirm your email</a>");
                    return Ok();
                }

                return BadRequest(result);
            }

            return BadRequest("Invalid Token");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest("Your link has expired or contains error. Please try again");
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

        //[Auth(Role.Admin, Role.OfficeWorker)]
        [HttpGet]
        [Route("GenerateRegisterToken")]
        public async Task<IActionResult> GenerateRegisterToken()
        {
            var parameter = await accountService.GenerateRegisterToken();

            if (string.IsNullOrEmpty(parameter))
            {
                return BadRequest();
            }

            return Ok(parameter);
        }

        [HttpGet]
        [Route("VerifyRegisterToken")]
        public IActionResult VerifyRegisterToken(string param)
        {
            var isParamValid = accountService.CheckRegisterToken(param);

            if (isParamValid)
                return Ok();

            return BadRequest("Invalid parameter");
        }
    }
}


