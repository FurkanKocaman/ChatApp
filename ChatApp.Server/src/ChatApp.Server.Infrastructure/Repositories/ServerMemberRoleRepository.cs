using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class ServerMemberRoleRepository : Repository<ServerMemberRole, ApplicationDbContext>, IServerMemberRoleRepository
{
    public ServerMemberRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
