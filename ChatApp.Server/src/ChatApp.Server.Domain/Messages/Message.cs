using ChatApp.Server.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Server.Domain.Messages;

public sealed class Message 
{
    public Message()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public string Content { get; set; } = default!;
    public string? ImageUrl { get; private set; }
    public string? FileUrl { get; private set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public string CreateuserName => GetCreateUserName();
    private string GetCreateUserName()
    {
        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

        AppUser user = userManager.Users.First(p => p.Id == CreateUserId);

        return user.FirstName + " " + user.LastName + " (" + user.Email + ")";
    }
    #endregion
}
