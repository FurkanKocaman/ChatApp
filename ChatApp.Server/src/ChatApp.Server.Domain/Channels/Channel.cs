using ChatApp.Server.Domain.Abstractions;

namespace ChatApp.Server.Domain.Channels;

public sealed class Channel : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public bool IsPublic { get; set; } = false;
}
