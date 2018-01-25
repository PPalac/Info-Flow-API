using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InfoFlow.Data.Models;
using InfoFlow.Core.Enums;
using InfoFlow.API.Services.Interfaces;
using InfoFlow.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Collections.Generic;

namespace InfoFlow.API.Services
{
    public class AccountService : IAccountService
    {
        private IConfiguration config;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountService(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.config = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
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


        public async Task<bool> RegisterStudentAsync(RegisterUserViewModel user)
        {
            var student = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Email = user.Email
            };

            var result = await userManager.CreateAsync(student, user.Password);

            if (result.Succeeded)
            {
                var res = await userManager.AddToRoleAsync(student, Role.Student.ToString());

                if (res.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> AddToRole(string userName, Role roleName)
        {
            var user = await userManager.FindByNameAsync(userName);

            var result = await userManager.AddToRoleAsync(user, roleName.ToString());

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
      
    }
}
