using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Conversations;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.ConversationParticipants;
public sealed class ConversationParticipant : Entity
{

    public Guid ParticipantId { get; set; }
    public AppUser? Participant { get; set; }
    public Guid ConversationId {get; set; }  
    public Conversation? Conversation { get; set; }
}
