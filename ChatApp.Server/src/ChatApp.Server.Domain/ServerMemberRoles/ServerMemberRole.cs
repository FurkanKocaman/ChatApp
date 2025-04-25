using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMembers;

namespace ChatApp.Server.Domain.ServerMemberRoles;
public sealed class ServerMemberRole
{
    public Guid ServerMemberId { get; set; }
    public ServerMember ServerMember { get; set; } = default!;

    public Guid AppRoleId { get; set; }
    public AppRole AppRole { get; set; } = default!;
}
