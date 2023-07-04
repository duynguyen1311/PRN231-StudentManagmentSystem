using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Utility;

namespace BusinessObject.Model.SeedData
{
    public static class IdentitySeedData
    {

        public static async Task Seed(SmsDbContext context, UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {

            context.Database.Migrate();
            if(await roleManager.FindByNameAsync(RoleConstant.ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConstant.ADMIN));
                await roleManager.CreateAsync(new IdentityRole(RoleConstant.TEACHER));
                await roleManager.CreateAsync(new IdentityRole(RoleConstant.STUDENT));
            }
            if(await userManager.FindByEmailAsync("admin@gmail.com") == null)
            {
                var user = new AppUser
                {
                    Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                    FullName = "Administrator",
                    Login = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Type = 2,
                    Activated = true,
                    LockoutEnabled = false,
                    PhoneNumber = "1234567890"
                };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, "Abc@123");
                    await userManager.AddToRoleAsync(user, RoleConstant.ADMIN);
                }
            }
        }
    }
}
