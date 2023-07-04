using BusinessObject.Model.SeedData;
using BusinessObject.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StudentManagingSystem_API.Configuration
{
    public static class DatabaseStartUp
    {
        public static IApplicationBuilder UseApplicationDatabase<T>(this IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.CreateScope())
            {
                //var services = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<SmsDbContext>();
                context.Database.Migrate();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IdentitySeedData.Seed(context, userMgr, roleMgr).Wait();
            }

            return app;
        }
    }
}
