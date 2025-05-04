using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Application.Roles;
public sealed record GetRoleDetailsByServerQuery(
    Guid serverId
    ) : IRequest<IQueryable<GetRoleDetailsByServerQueryResponse>>;

public sealed class GetRoleDetailsByServerQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public int UserCount { get; set; }
    public decimal Level { get;set; }
    public int AccessibleChannelCount { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
}

internal sealed class GetRoleDetailsByServerQueryHandler(
    RoleManager<AppRole> roleManager
    ,UserManager<AppUser> userManager
    , IChannelRolePermissionRepository channelRolePermissionRepository
    , IServerMemberRoleRepository serverMemberRoleRepository
    ) : IRequestHandler<GetRoleDetailsByServerQuery, IQueryable<GetRoleDetailsByServerQueryResponse>>
{
    public async Task<IQueryable<GetRoleDetailsByServerQueryResponse>> Handle(GetRoleDetailsByServerQuery request, CancellationToken cancellationToken)
    {

        var roles = await roleManager.Roles
             .Where(p => p.ServerId == request.serverId && !p.IsDeleted)
             .ToListAsync(cancellationToken);

        var memberCounts = await serverMemberRoleRepository
            .Where(x => x.AppRole.ServerId == request.serverId)
            .GroupBy(x => x.AppRoleId)
            .Select(g => new { RoleId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var accessibleChannelCounts = await channelRolePermissionRepository
            .Where(x => x.Role.ServerId == request.serverId)
            .GroupBy(x => x.RoleId)
            .Select(g => new { RoleId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var responses = new List<GetRoleDetailsByServerQueryResponse>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            var memberCount = memberCounts.FirstOrDefault(x => x.RoleId == role.Id)?.Count ?? 0;
            var channelCount = accessibleChannelCounts.FirstOrDefault(x => x.RoleId == role.Id)?.Count ?? 0;
            var createUser = await userManager.Users.Where(p => p.Id == role.CreateUserId).FirstOrDefaultAsync(cancellationToken);
            var updateUser = await userManager.Users.Where(p => p.Id == role.UpdateUserId).FirstOrDefaultAsync(cancellationToken);

            responses.Add(new GetRoleDetailsByServerQueryResponse
            {
                Id = role.Id,
                Name = role.Name!,
                Level = role.Level,
                UserCount = memberCount,
                AccessibleChannelCount = channelCount,
                Permissions = claims.Select(c => c.Value).ToList(),
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                CreateUserId = role.CreateUserId,
                CreateUserName = createUser != null ? createUser.FullName  + "(" + createUser.Email +")" : "null", 
                UpdateAt = role.UpdateAt,
                UpdateUserId = role.UpdateUserId,
                UpdateUserName = updateUser != null ? updateUser.FullName + "(" + updateUser.Email + ")" : "null",
                IsDeleted = role.IsDeleted,
                DeleteAt = role.DeleteAt
            });
        }

        return responses.AsQueryable();
    }
}
