using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Servers;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.RoleClaim;
using System.Data;

namespace ChatApp.Server.Application.Servers;
public sealed record GetUserModeratedServersQuery(
    ) : IRequest<IQueryable<GetUserModeratedServersQueryResponse>>;

public sealed class GetUserModeratedServersQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public int ChannelCount { get; set; }
    public int MemberCount { get; set; }
    public int RoleCount { get; set; }
    public string AccessType { get; set; } = default!;
    public string Status { get; set; } = default!;
    //public DateTimeOffset? JoinedAt { get; set; }
}

internal sealed class GetUserModeratedServersQueryHandler(
    ICurrentUserService currentUserService,
    IServerMemberRepository serverMemberRepository,
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager
    ) : IRequestHandler<GetUserModeratedServersQuery, IQueryable<GetUserModeratedServersQueryResponse>>
{
    public async Task<IQueryable<GetUserModeratedServersQueryResponse>> Handle(GetUserModeratedServersQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Enumerable.Empty<GetUserModeratedServersQueryResponse>().AsQueryable();

        var serverMemberships = serverMemberRepository.Where(p => p.UserId == userId && !p.IsDeleted).Include(p => p.Server).ThenInclude(p => p!.Channels).Include(p => p.Server).ThenInclude(p => p!.Roles).Include(p => p.ServerMemberRoles).ThenInclude(p => p.AppRole).ToList();

        if(serverMemberships is null || !serverMemberships.Any())
            return Enumerable.Empty<GetUserModeratedServersQueryResponse>().AsQueryable();

        List<Domain.Servers.Server> servers = new List<Domain.Servers.Server>();

        foreach(var serverMembership in serverMemberships)
        {
            foreach(var serverMemberRole in serverMembership.ServerMemberRoles)
            {
                if(serverMemberRole is not null)
                {
                    var claims = await roleManager.GetClaimsAsync(serverMemberRole.AppRole);

                    if (claims.Any(p => p.Value == Permissions.EditServer))
                    {
                        if(!servers.Any(p => p == serverMembership.Server!))
                        {
                            servers.Add(serverMembership.Server!);
                        }
                    }
                }
            }
        }

        var responses = new List<GetUserModeratedServersQueryResponse>();

        foreach (var server in servers)
        {
            var channelCount = server.Channels.Count;
            var memberCount = serverMemberRepository.Where(p => p.ServerId == server.Id && !p.IsDeleted).Count();
            var roleCount = server.Roles.Count;

            var createUser = await userManager.Users.Where(p => p.Id == server.CreateUserId).FirstOrDefaultAsync(cancellationToken);
            var updateUser = await userManager.Users.Where(p => p.Id == server.UpdateUserId).FirstOrDefaultAsync(cancellationToken);

            var response = new GetUserModeratedServersQueryResponse
            {
                Id = server.Id,
                Name = server.Name,
                IconUrl = server.IconUrl,
                ChannelCount = channelCount,
                MemberCount = memberCount,
                RoleCount = roleCount,
                AccessType = "",
                Status = server.IsActive ? "Active" : "Passive",
                IsActive = server.IsActive,
                CreatedAt = server.CreatedAt,
                CreateUserId = server.CreateUserId,
                CreateUserName = createUser != null ? createUser.FullName + "(" + createUser.Email + ")" : "null",
                UpdateAt = server.UpdateAt,
                UpdateUserId = server.UpdateUserId,
                UpdateUserName = updateUser != null ? updateUser.FullName + "(" + updateUser.Email + ")" : "null",
                IsDeleted = server.IsDeleted,
                DeleteAt = server.DeleteAt,
            };

            responses.Add(response);
        }

        return responses.AsQueryable();
    }
}

