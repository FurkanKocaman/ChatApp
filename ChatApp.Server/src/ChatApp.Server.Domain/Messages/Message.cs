using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;

namespace ChatApp.Server.Domain.Messages;

public sealed class Message : Entity
{
    public Guid ChannelId { get; set; }
    public Channel? Channel { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Text;
    public string? ImageUrl { get; private set; }
    public string? FileUrl { get; private set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
}

public enum MessageType 
{
    Text,
    Image,
    File,
    System 
}