using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InfoFlowAPI.Data.Models;
using InfoFlowAPI.Services.Interfaces;
using InfoFlowAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InfoFlowAPI.Services
{
    public class AccountService : IAccountService
    {
        private IConfiguration config;
        private UserManager<User> userManager;

        public AccountService(IConfiguration config, UserManager<User> userManager)
        {
            this.config = config;
            this.userManager = userManager;
        }

        public async Task<UserViewModel> AuthenticateAsync(LoginViewModel login)
        {
            UserViewModel registeredUser = null;
            var user = await userManager.FindByNameAsync(login.Username);

            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, login.Password))
                {
                    registeredUser = new UserViewModel { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName };
                }
            }
            return registeredUser;
        }

        public string BuildToken(UserViewModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterUserAsync(RegisterUserViewModel user)
        {
            var result = await userManager.CreateAsync(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Email = user.Email
            }, user.Password);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
