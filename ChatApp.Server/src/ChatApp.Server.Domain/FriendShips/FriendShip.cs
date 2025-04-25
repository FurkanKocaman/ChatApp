using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.FriendShips;
public sealed class FriendShip : Entity
{
    public Guid RequesterId { get; set; }
    public AppUser? Requester { get; set; }
    public Guid ReceiverId { get; set; }
    public AppUser? Receiver { get; set; }
    public FriendshipStatus Status { get; set; }
}

public enum FriendshipStatus
{
    Pending, 
    Accepted, 
    Declined,
    Blocked    
}

