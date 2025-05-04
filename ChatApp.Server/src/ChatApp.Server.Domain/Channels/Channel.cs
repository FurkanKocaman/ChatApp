using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Messages;

namespace ChatApp.Server.Domain.Channels;
public sealed class Channel : Entity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public ChannelType Type { get; set; }
    public Guid ServerId { get; set; }
    public Servers.Server? Server { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<Message> Messages { get;set; } = new List<Message>();
    public ICollection<ChannelRolePermission> RolePermissions { get; set; } = new List<ChannelRolePermission>();
}

public enum ChannelType
{
    Text,
    Voice
}