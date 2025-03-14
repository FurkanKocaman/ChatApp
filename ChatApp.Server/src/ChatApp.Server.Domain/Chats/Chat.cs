using ChatApp.Server.Domain.Abstractions;

namespace ChatApp.Server.Domain.Chats;

public sealed class Chat : Entity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid ChannelId { get; set; }
    public IEnumerable<string> AllowedRoles { get; set; } = new List<string>();
}
