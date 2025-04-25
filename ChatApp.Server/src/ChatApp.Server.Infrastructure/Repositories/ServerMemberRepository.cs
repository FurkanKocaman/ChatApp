using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class ServerMemberRepository : Repository<ServerMember, ApplicationDbContext>, IServerMemberRepository
{
    public ServerMemberRepository(ApplicationDbContext context) : base(context)
    {
    }
}