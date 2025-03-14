namespace ChatApp.Server.Domain.ChannelMemberships;

public sealed class ChannelMembership
{
    public Guid ChannelId { get; set; }
    public Guid UserId { get; set; }
    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    #endregion
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}
