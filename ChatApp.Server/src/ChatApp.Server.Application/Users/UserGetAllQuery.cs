using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Users;

public sealed record UserGetAllQuery() : IRequest<IQueryable<UserGetAllQueryResponse>>;

public sealed class UserGetAllQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? ProfileImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public UserStatus Status { get; set; }
    public DateTimeOffset? BirthOfDate { get; set; }
    public bool? Gender { get; set; }
    public DateTimeOffset? LastActive { get; set; }
}
public sealed class UserGetAllQueryHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserGetAllQuery, IQueryable<UserGetAllQueryResponse>>
{
    public Task<IQueryable<UserGetAllQueryResponse>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from user in userManager.Users
                        join create_user in userManager.Users.AsQueryable() on user.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on user.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new UserGetAllQueryResponse
                        {
                            Id = user.Id,
                            FullName = user.FullName,
                            UserName = user.UserName!,
                            Email = user.Email!,
                            BirthOfDate = user.BirthOfDate,
                            Status = user.Status,
                            Gender = user.Gender,
                            LastActive = user.LastActive,
                            CreatedAt = user.CreatedAt,
                            DeleteAt = user.DeleteAt,
                            IsDeleted = user.IsDeleted,
                            UpdateAt = user.UpdateAt,
                            CreateUserId = user.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateUserId = user.UpdateUserId,
                            UpdateUserName = user.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" +
                            update_users.Email + ")",
                        });
        return Task.FromResult(response);
    }
}