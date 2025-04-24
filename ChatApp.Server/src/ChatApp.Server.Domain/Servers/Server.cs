using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.Servers;
public sealed class Server : Entity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public Guid OwnerId { get; set; }
    public AppUser? Owner { get; set; }
    public ICollection<ServerMember> Members { get; set; } = new List<ServerMember>();
    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
