using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class MessageRepository : Repository<Message, ApplicationDbContext>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}

