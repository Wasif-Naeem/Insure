using Health_Insurance.Models;
using Microsoft.AspNetCore.Identity;
namespace Health_Insurance.Data
{
    public static class SeedAdmin
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleMgr.RoleExistsAsync("Admin"))
                await roleMgr.CreateAsync(new IdentityRole("Admin"));
            if (!await roleMgr.RoleExistsAsync("Employee"))
                await roleMgr.CreateAsync(new IdentityRole("Employee"));

            var adminEmail = "admin@insurance.com";
            var adminPassword = "Admin@123";

            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Insure Admin"
                };

                var result = await userMgr.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }
    }


}
