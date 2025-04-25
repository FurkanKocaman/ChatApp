using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Conversations;
using ChatApp.Server.Domain.Messages;

namespace ChatApp.Server.Domain.DirectMessages;
public sealed class DirectMessage : Entity
{
    public Guid ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Text;
    public string? ImageUrl { get; private set; }
    public string? FileUrl { get; private set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
}
