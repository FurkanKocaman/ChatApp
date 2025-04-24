using ChatApp.Server.Domain.ConversationParticipants;
using ChatApp.Server.Domain.FriendShips;
using ChatApp.Server.Domain.ServerMembers;
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
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Offline;
    public string? CustomStatus { get; set; }
    public DateTimeOffset? BirthOfDate { get; set; }
    public bool? Gender { get; set; }
    public DateTimeOffset? LastActive { get; set; }
    public ICollection<ServerMember> ServerMemberships { get; set; } = new List<ServerMember>();
    public ICollection<ConversationParticipant> Conversations { get; set; } = new List<ConversationParticipant>();
    public ICollection<FriendShip> FriendShips { get; set; } = new List<FriendShip>();


    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}

public enum UserStatus
{
    Online,
    Offline,
    Idle,     
    DoNotDisturb 
}
