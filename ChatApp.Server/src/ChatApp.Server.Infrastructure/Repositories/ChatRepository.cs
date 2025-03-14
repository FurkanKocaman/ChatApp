using ChatApp.Server.Domain.Chats;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;

internal sealed class ChatRepository : Repository<Chat, ApplicationDbContext>, IChatRepository
{
    public ChatRepository(ApplicationDbContext context) : base(context)
    {
    }
}
