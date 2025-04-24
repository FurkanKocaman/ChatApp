using ChatApp.Server.Domain.Conversations;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal class ConversationRepository : Repository<Conversation, ApplicationDbContext>, IConversationRepository
{
    public ConversationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
