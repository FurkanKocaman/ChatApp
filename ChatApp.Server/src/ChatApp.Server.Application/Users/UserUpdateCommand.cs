using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Users;

public sealed record UserUpdateCommand(
    Guid Id,
    string FirstName,
    string Lastname,
    string Username,
    string Email,
    DateTimeOffset BirtOfDate,
    bool? Gender) : IRequest<Result<string>>;

internal sealed class UserUpdateCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if(user == null)
            return Result<string>.Failure("User not found");

        user.FirstName = request.FirstName;
        user.LastName = request.Lastname;
        user.UserName = request.Username;
        user.Email = request.Email;
        user.Gender = request.Gender;
        user.BirthOfDate = request.BirtOfDate;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<string>.Failure("An error occured during update");
        }
        return Result<string>.Succeed($"{user.UserName} updated successfully");
    }
}
