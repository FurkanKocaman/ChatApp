using ChatApp.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace ChatApp.Server.Application.Roles;
public sealed record RoleUpdateCommand(
    Guid Id,
    string Name,
    decimal Level,
    string? ColorHex,
    List<string> Claims
    ) : IRequest<Result<string>>;

internal sealed class RoleUpdateCommandHandler(
    RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());

        if (role is null)
            return Result<string>.Failure("Role not found");

        role.Name = request.Name;
        role.Level = request.Level;
        role.ColorHex = request.ColorHex;

        var existingClaims = await roleManager.GetClaimsAsync(role);

        var toRemove = existingClaims.Where(p => !request.Claims.Contains(p.Value)).ToList();
        foreach ( var claim in toRemove)
        {
            await roleManager.RemoveClaimAsync(role, claim);
        }

        var toAdd = request.Claims.Where(p => existingClaims.Any(c => c.Value == p)).ToList();

        foreach( var claim in toAdd)
        {
            await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permission", claim));
        }

        return Result<string>.Succeed("Role updated successfully");
    }
}
