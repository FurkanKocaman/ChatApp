using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Roles;
public sealed record RoleCreateCommand(
    string Name,
    Guid ServerId,
    decimal Level,
    string? ColorHex,
    List<string> Claims
    ) : IRequest<Result<string>>;

internal sealed class RoleCreateCommandHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        AppRole role = new()
        {
            Name = request.Name,
            Level = request.Level,
            ColorHex = request.ColorHex,
            ServerId = request.ServerId,
            CreateUserId = userId.Value,
            CreatedAt = DateTimeOffset.Now
        };

        IdentityResult result =  await roleManager.CreateAsync( role );

        if (!result.Succeeded)
            return Result<string>.Failure("Role couldn't created");

        foreach(var claim in request.Claims)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            if (!claims.Contains(new System.Security.Claims.Claim("permission", claim)))
            {
                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permission", claim));
            }
        }

        return Result<string>.Succeed("Role created successfully");
    }
}
