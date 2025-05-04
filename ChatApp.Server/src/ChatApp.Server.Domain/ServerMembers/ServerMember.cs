using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.ServerMembers;
public sealed class ServerMember : Entity
{
    public Guid ServerId { get; set; }
    public Servers.Server? Server { get; set; }
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string? Nickname { get; set; }
    public Guid? ActiveRoleId { get; set; } 
    public AppRole? ActiveRole { get; set; }
    public ICollection<ServerMemberRole> ServerMemberRoles { get; set; } = new List<ServerMemberRole>();
}
