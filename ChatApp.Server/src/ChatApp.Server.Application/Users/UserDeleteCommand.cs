using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Users;

public sealed record UserDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class UserDeleteCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            return Result<string>.Failure("User not found");

        user.IsDeleted = true;
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Result<string>.Failure("Something went wrong during delete");

        return Result<string>.Succeed($"{user.UserName} deleted successfully");
    }
}
