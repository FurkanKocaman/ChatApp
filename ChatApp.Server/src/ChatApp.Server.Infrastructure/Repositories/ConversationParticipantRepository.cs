using ChatApp.Server.Domain.ConversationParticipants;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class ConversationParticipantRepository : Repository<ConversationParticipant, ApplicationDbContext>, IConversationParticipantRepository
{
    public ConversationParticipantRepository(ApplicationDbContext context) : base(context)
    {
    }
}
