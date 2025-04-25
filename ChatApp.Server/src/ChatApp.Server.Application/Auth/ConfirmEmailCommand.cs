using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Auth;
public sealed record ConfirmEmailCommand(
    string UserId,
    string Token
    ) :IRequest<Result<string>>;

internal sealed class ConfirmEmailCommandHandler(
    UserManager<AppUser> userManager
    ) : IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return Result<string>.Failure("User not found");

        var result = await userManager.ConfirmEmailAsync(user,request.Token);

        if (!result.Succeeded)
            return Result<string>.Failure("Mail could not confirmed");

        return Result<string>.Succeed("Mail confirmed.");
    }
}
