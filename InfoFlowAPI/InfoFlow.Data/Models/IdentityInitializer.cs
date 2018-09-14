using InfoFlow.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace InfoFlow.Data.Models
{
    public static class IdentityInitializer
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new User
                {
                    FirstName = "SU",
                    LastName = "Adminowski",
                    UserName = "admin",
                    Email = "piotrekpalac1@gmail.com"
                };

                var result = userManager.CreateAsync(user, "Admin1").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Role.Admin.ToString()).Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Role.Admin.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.Admin.ToString() }).Wait();
            }

            if (!roleManager.RoleExistsAsync(Role.OfficeWorker.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.OfficeWorker.ToString() }).Wait();
            }

            if (!roleManager.RoleExistsAsync(Role.Teacher.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.Teacher.ToString() }).Wait();
            }

            if (!roleManager.RoleExistsAsync(Role.Student.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.Student.ToString() }).Wait();
            }

            if (!roleManager.RoleExistsAsync(Role.GroupHead.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.GroupHead.ToString() }).Wait();
            }

            if (!roleManager.RoleExistsAsync(Role.YearHead.ToString()).Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = Role.YearHead.ToString() }).Wait();
            }
        }
    }
}
