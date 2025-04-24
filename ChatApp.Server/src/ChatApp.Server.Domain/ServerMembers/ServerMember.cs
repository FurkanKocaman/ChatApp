using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.ServerMembers;
public sealed class ServerMember : Entity
{
    public Guid ServerId { get; set; }
    public Servers.Server? Server { get; set; }
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string? Nickname { get; set; }
    public ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
