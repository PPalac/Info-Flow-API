using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InfoFlow.API.Services.Interfaces;
using InfoFlow.API.ViewModels;
using InfoFlow.Core.Enums;
using InfoFlow.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InfoFlow.API.Services
{
    public class AccountService : IAccountService
    {
        private IConfiguration config;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IEmailSenderService emailSender;

        public AccountService(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailSenderService emailSender)
        {
            this.config = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

        public async Task<UserViewModel> AuthenticateAsync(LoginViewModel login)
        {
            UserViewModel registeredUser = null;
            var user = await userManager.FindByNameAsync(login.Username);

            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, login.Password))
                {
                    var roles = userManager.GetRolesAsync(user).Result;

                    registeredUser = new UserViewModel { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, Roles = roles.ToList()};
                }
            }
            return registeredUser;
        }

        public string BuildToken(UserViewModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roleClaims = new List<Claim>();

            foreach (var role in user.Roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            claims.AddRange(roleClaims);

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(int.Parse(config["Jwt:ExpirationTime"])),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string> RegisterStudentAsync(RegisterUserViewModel user)
        {
            var student = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Email = user.Email
            };

            var result = await userManager.CreateAsync(student);

            if (result.Succeeded)
            {
                var res = await userManager.AddToRoleAsync(student, Role.Student.ToString());

                if (res.Succeeded)
                {
                    return res.Succeeded.ToString();
                }

                return res.Errors.FirstOrDefault().Description;
            }

            return result.Errors.FirstOrDefault().Description;
        }

        public async Task<string> AddToRole(string userName, Role roleName)
        {
            var user = await userManager.FindByNameAsync(userName);

            var result = await userManager.AddToRoleAsync(user, roleName.ToString());

            return result.Succeeded ? result.Succeeded.ToString() : result.Errors.ToString();
        }


    }
}
