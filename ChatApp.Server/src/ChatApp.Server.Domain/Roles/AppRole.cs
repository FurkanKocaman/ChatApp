using ChatApp.Server.Domain.ServerMemberRoles;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Domain.Roles;

public sealed class AppRole:IdentityRole<Guid>
{
    public Guid ServerId { get; set; } = default!;
    public Servers.Server? Server { get; set; }
    public decimal Level { get; set; }
    public string? ColorHex { get; set; }
    public ICollection<ServerMemberRole> ServerMemberRoles { get; set; } = new List<ServerMemberRole>();
    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}
