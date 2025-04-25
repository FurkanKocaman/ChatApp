using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ConversationParticipants;

namespace ChatApp.Server.Domain.Conversations;
public sealed class Conversation : Entity
{
    public string Name { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
}
