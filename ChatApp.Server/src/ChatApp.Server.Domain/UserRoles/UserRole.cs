using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.UserRoles;
public sealed class UserRole: Entity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid RoleId { get; set; }
    public AppRole? Role { get; set; }
    public Guid ServerId { get; set; }
    public Servers.Server? Server{ get; set; }
}
