using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentAssertions;
using InfoFlow.Data.Models;
using InfoFlow.API.Services;
using InfoFlow.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace InfoFlowAPI.Tests.ServicesTests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private AccountService service;

        public AccountServiceTests()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(conf => conf["Jwt:Key"]).Returns("qwertyuiopasdfgh");
            config.Setup(conf => conf["Jwt:Issuer"]).Returns("localhost:00000");
            config.Setup(conf => conf["Jwt:ExpirationTime"]).Returns("10");

            var userManager = new UserManager<User>( //Todo: Create new class for mock configs (extension methods maybe?)!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new List<IUserValidator<User>>(),
                new List<IPasswordValidator<User>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object
                );

            var roleManager = new RoleManager<IdentityRole>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new List<IRoleValidator<IdentityRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object
                );

            //this.service = new AccountService(config.Object, userManager, roleManager);
        }
        [Test]
        public void BuildToken_UserData_ShouldCreateJWT()
        {
            //Arrange
            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            UserViewModel user = new UserViewModel
            {
                FirstName = "Jan",
                LastName = "Kowalsky",
                Roles = new List<string>() { "Teacher" },
                UserName = "jkowalsky"
            };

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "localhost:00000",
                ValidAudience = "localhost:00000",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfgh"))

            };

            //Act
            var token = service.BuildToken(user);
            var Userr = handler.ValidateToken(token, validationParameters, out validatedToken);

            //Assert
            Userr.Identity.IsAuthenticated.Should().BeTrue();
        }
    }
}
