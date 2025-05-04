using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelGetQuery(
    Guid Id
    ) : IRequest<Result<ChannelGetQueryResponse>>;

public sealed class ChannelGetQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public List<Guid> RoleIds { get; set; } = new List<Guid>();    
    public string? IconUrl { get; set; }
    public ChannelType ChannelType { get; set; }
}

internal sealed class ChannelGetQueryHandler(
    IChannelRepository channelRepository,
    IChannelRolePermissionRepository channelRolePermissionRepository,
    UserManager<AppUser> userManager
    ) : IRequestHandler<ChannelGetQuery, Result<ChannelGetQueryResponse>>
{
    public async Task<Result<ChannelGetQueryResponse>> Handle(ChannelGetQuery request, CancellationToken cancellationToken)
    {
        var channel =  channelRepository.Where(p => p.Id == request.Id && !p.IsDeleted);

        if (channel is null)
            return Result<ChannelGetQueryResponse>.Failure("Channel not found");

        var roles = await channelRolePermissionRepository.Where(p => p.ChannelId == channel.FirstOrDefault()!.Id).Include(p => p.Role).Select(p => p.Role.Id).ToListAsync(cancellationToken);

        var response = await channel
                .GroupJoin(userManager.Users,
                    channel => channel.CreateUserId, 
                    createUser => createUser.Id,
                    (channel, createUsers) => new {channel, createUsers})
                .SelectMany(
                    cc => cc.createUsers.DefaultIfEmpty(),
                    (cc, createUser) => new {cc.channel, createUser})
                .GroupJoin(userManager.Users,
                    cc => cc.channel.UpdateUserId,
                    updateUser => updateUser.Id,
                    (cc, updateUsers) => new { cc.channel, cc.createUser, updateUsers })
                .SelectMany(
                    cc => cc.updateUsers.DefaultIfEmpty(),
                     (cc, updateUser) => new ChannelGetQueryResponse
                    {
                        Id = cc.channel.Id,
                        Name = cc.channel.Name,
                        Description = cc.channel.Description,
                        IsPublic = cc.channel.IsPublic,
                        RoleIds = roles, 
                        IconUrl = cc.channel.IconUrl,
                        ChannelType = cc.channel.Type,
                        IsActive = cc.channel.IsActive,
                        CreatedAt = cc.channel.CreatedAt,
                        CreateUserId = cc.createUser!= null ? cc.createUser.Id : Guid.Empty,
                        CreateUserName = cc.createUser != null ? cc.createUser.FirstName + " " + cc.createUser.LastName + " (" + cc.createUser.Email + ")" : "null",
                        UpdateAt = cc.channel.UpdateAt,
                        UpdateUserId = updateUser != null ? updateUser.Id : Guid.Empty,
                        UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : "null",
                        IsDeleted = cc.channel.IsDeleted,
                        DeleteAt = cc.channel.DeleteAt,
                    }).FirstOrDefaultAsync();


        return Result<ChannelGetQueryResponse>.Succeed(response!);


    }
}

