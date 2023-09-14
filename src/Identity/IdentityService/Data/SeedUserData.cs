using Microsoft.AspNetCore.Identity;

namespace IdentityService.Data
{
    public static class SeedUserData
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            var context = serviceScope?.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (!context.Users.Any())
            {
                var roleContext = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                roleContext.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                }).Wait();
                roleContext.CreateAsync(new IdentityRole()
                {
                    Name = "Customer"
                }).Wait();

                foreach (var user in AddUsers())
                {
                    var ss = context.CreateAsync(user, "123321@Aa").Result;
                    if (user.UserName == "sysAdmin")
                        context.AddToRoleAsync(user, "Admin").Wait();
                    else if (user.UserName == "manager")
                        context.AddToRoleAsync(user, "Customer").Wait();
                }
            }
        }

        private static List<IdentityUser> AddUsers()
        {
            return new List<IdentityUser>()
            {
                new IdentityUser()
                {
                    UserName = "sysAdmin",
                    Email = "sysAdmin@gmail.com",
                    EmailConfirmed = true
                },
                new IdentityUser()
                {
                    UserName = "manager",
                    Email = "manager@gmail.com",
                    EmailConfirmed = true
                }
            };
        }
    }
}
