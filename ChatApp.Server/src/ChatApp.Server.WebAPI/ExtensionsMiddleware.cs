using ChatApp.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            if(!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    Id = Guid.Parse("95b12cf8-5095-4b89-bc0d-07e4a826c74c"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Furkan",
                    LastName = "Kocaman",
                    EmailConfirmed = true,
                    CreatedAt = DateTimeOffset.Now,
                };
                user.CreateUserId = user.Id;
                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}
