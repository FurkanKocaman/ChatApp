using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Domain.Users;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? ProfileImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public bool isOnline { get; set; } = false;
    public DateTimeOffset BirthOfDate { get; set; }
    public bool? Gender { get; set; }
    public DateTimeOffset? LastActive { get; set; }

    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}
