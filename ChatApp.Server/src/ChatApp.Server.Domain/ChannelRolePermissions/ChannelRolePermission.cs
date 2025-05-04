using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Roles;

namespace ChatApp.Server.Domain.ChannelRolePermissions;
public sealed class ChannelRolePermission
{
    public Guid ChannelId { get; set; }
    public Channel Channel { get; set; } = default!;

    public Guid RoleId { get; set; }
    public AppRole Role { get; set; } = default!;

    public bool CanEdit { get; set; } = true;
}
