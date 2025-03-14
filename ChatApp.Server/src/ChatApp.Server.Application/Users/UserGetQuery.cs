using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ChatApp.Server.Application.Users;

public sealed record UserGetQuery(Guid Id) : IRequest<Result<UserGetQueryResponse?>>;

public sealed class UserGetQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? ProfileImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public bool isOnline { get; set; } = false;
    public DateTimeOffset BirthOfDate { get; set; }
    public bool? Gender { get; set; }
    public DateTimeOffset? LastActive { get; set; }
}

internal sealed class UserGetQueryHandler(UserManager<AppUser> userManager)
    : IRequestHandler<UserGetQuery, Result<UserGetQueryResponse?>>
{
    public async Task<Result<UserGetQueryResponse?>> Handle(UserGetQuery request, CancellationToken cancellationToken)
    {
        var user = await (from u in userManager.Users
                          where u.Id == request.Id
                          join create_user in userManager.Users on u.CreateUserId equals create_user.Id
                          join update_user in userManager.Users.AsQueryable() on u.UpdateUserId equals update_user.Id
                          into update_user
                          from update_users in update_user.DefaultIfEmpty()
                          select new UserGetQueryResponse
                          {
                              Id = u.Id,
                              FullName = u.FullName,
                              UserName = u.UserName!,
                              Email = u.Email!,
                              ProfileImageUrl = u.ProfileImageUrl,
                              RefreshToken = u.RefreshToken,
                              isOnline = u.isOnline,
                              BirthOfDate = u.BirthOfDate,
                              Gender = u.Gender,
                              LastActive = u.LastActive,
                              CreatedAt = u.CreatedAt,
                              CreateUserId = create_user.Id,
                              CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                              UpdateAt = u.UpdateAt,
                              UpdateUserId = update_users.Id,
                              UpdateUserName = u.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                              IsDeleted = u.IsDeleted,
                              DeleteAt = u.DeleteAt,
                          })
                   .FirstOrDefaultAsync(cancellationToken);


        if (user is null)
            return Result<UserGetQueryResponse?>.Failure("User not found");

        return Result<UserGetQueryResponse?>.Succeed(user);
    }
}
