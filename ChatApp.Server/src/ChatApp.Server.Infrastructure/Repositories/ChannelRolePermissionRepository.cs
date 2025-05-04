using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class ChannelRolePermissionRepository : Repository<ChannelRolePermission, ApplicationDbContext>, IChannelRolePermissionRepository
{
    public ChannelRolePermissionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
