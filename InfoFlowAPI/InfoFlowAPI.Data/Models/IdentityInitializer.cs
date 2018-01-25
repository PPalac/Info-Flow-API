using InfoFlow.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace InfoFlow.Data.Models
{
    public static class IdentityInitializer
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

        }

        public static void SeedUsers(UserManager<User> userManager)
        {
        }

        public static async System.Threading.Tasks.Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync(Role.Admin.ToString()))
            {

            }
        }
    }
}
