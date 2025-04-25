using ChatApp.Server.Domain.Servers;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class ServerRepository : Repository<Domain.Servers.Server, ApplicationDbContext>, IServerRepository
{
    public ServerRepository(ApplicationDbContext context) : base(context)
    {
    }
}