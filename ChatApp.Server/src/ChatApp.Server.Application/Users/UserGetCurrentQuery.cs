using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Users;
public sealed record UserGetCurrentQuery(
    ) : IRequest<UserGetCurrentQueryResponse>;

public sealed class UserGetCurrentQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Status { get; set; }
    public string? CustomStatus { get; set; }
    public DateTimeOffset? BirthOfDate { get; set; }
    public string? Gender { get; set; }
    public DateTimeOffset? LastActive { get; set; }
    //public ICollection<ServerMember> ServerMemberships { get; set; } = new List<ServerMember>();
    //public ICollection<ConversationParticipant> Conversations { get; set; } = new List<ConversationParticipant>();
    //public ICollection<FriendShip> FriendShips { get; set; } = new List<FriendShip>();

    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; }
    public string CreateUserName { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
}

internal sealed class UserGetCurrentQueryHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<UserGetCurrentQuery, UserGetCurrentQueryResponse>
{
    public async Task<UserGetCurrentQueryResponse> Handle(UserGetCurrentQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            throw new UnauthorizedAccessException("User not found.");

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            throw new UnauthorizedAccessException("User not found");

        var response = userManager.Users.Where(p => p.Id == userId.Value)
                .GroupJoin(userManager.Users,
                    user => user.CreateUserId,
                    createUser => createUser.Id,
                    (user, createUsers) => new { user, createUsers })
                .SelectMany(
                    uc => uc.createUsers.DefaultIfEmpty(),
                    (uc, createUser) => new { uc.user, createUser })
                .GroupJoin(userManager.Users,
                    uc => uc.user.UpdateUserId,
                    updateUsers => updateUsers.Id,
                    (uc, updateUsers) => new { uc.user, uc.createUser, updateUsers })
                .SelectMany(
                    uc => uc.updateUsers.DefaultIfEmpty(),
                    (uc, updateUser) => new UserGetCurrentQueryResponse
                    {
                        Id = uc.user.Id,
                        FirstName = uc.user.FirstName,
                        LastName = uc.user.LastName,
                        DisplayName = uc.user.DisplayName,
                        AvatarUrl = uc.user.AvatarUrl,
                        Status = uc.user.Status == UserStatus.Online ? "Online" : uc.user.Status == UserStatus.Offline ? "Offline" : uc.user.Status == UserStatus.Idle ? "Idle" : uc.user.Status == UserStatus.DoNotDisturb ? "DoNotDisturb" : "null",
                        CustomStatus = uc.user.CustomStatus,
                        BirthOfDate = uc.user.BirthOfDate,
                        Gender = uc.user.Gender != null ? uc.user.Gender.Value ? "Man" : "Woman" : "null",
                        LastActive = uc.user.LastActive,
                        CreatedAt = uc.user.CreatedAt,
                        CreateUserId = uc.user.CreateUserId,
                        CreateUserName = uc.createUser != null ? uc.createUser.FirstName + " " + uc.createUser.LastName + " (" + uc.createUser.Email + ")" : "null",
                        UpdateAt = uc.user.UpdateAt,
                        UpdateUserId = uc.user.UpdateUserId,
                        UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : "null",
                        IsDeleted = uc.user.IsDeleted,
                        DeleteAt = uc.user.DeleteAt,
                    }).FirstOrDefault();

        if(response is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        return response;
    }
}
