using ChatApp.Server.Domain.DirectMessages;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class DirectMessageRepository : Repository<DirectMessage, ApplicationDbContext>, IDirectMessageRepository
{
    public DirectMessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}

