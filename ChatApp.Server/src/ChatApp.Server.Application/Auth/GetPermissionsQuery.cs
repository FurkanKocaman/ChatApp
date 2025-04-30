using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.ServerMembers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ChatApp.Server.Application.Auth;
public sealed record GetPermissionsQuery(
    Guid serverId
    ) : IRequest<Result<string[]>>;

internal sealed class GetPermissionsQueryHandler(
    ICurrentUserService currentUserService,
    IServerMemberRepository serverMemberRepository,
    RoleManager<AppRole> roleManager
    ) : IRequestHandler<GetPermissionsQuery, Result<string[]>>
{
    public async Task<Result<string[]>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string[]>.Failure("User not found");

        var serverMember = await serverMemberRepository.Where(p => p.ServerId == request.serverId && p.UserId == userId.Value && !p.IsDeleted).Include(p => p.ServerMemberRoles).ThenInclude(p => p.AppRole).FirstOrDefaultAsync();

        if (serverMember is null)
            return Result<string[]>.Failure("You aren't member of this server");

        var serverMemberRoles = serverMember.ServerMemberRoles.ToList();

        List<string> permissions = new List<string>();

        foreach ( var serverMemberRole in serverMemberRoles)
        {
            if(serverMemberRole is not null)
            {
                var role = serverMemberRole.AppRole;
                var claims = await roleManager.GetClaimsAsync(role);

                permissions.AddRange(claims.Select(p => p.Value));
            }
        }

        return Result<string[]>.Succeed(permissions.ToArray());
       
    }
}

