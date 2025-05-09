﻿using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Application.Channels;
public sealed record GetChannelSummariesQuery(
    Guid serverId,
    int page,
    int pageSize
    ) : IRequest<List<GetChannelSummariesQueryResponse>>;

public sealed class GetChannelSummariesQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public string? IconUrl { get; set; }
    public ChannelType ChannelType { get; set; }
}

internal sealed class GetChannelSummariesQueryhandler(
    ICurrentUserService currentUserService,
    IChannelRolePermissionRepository channelRolePermissionRepository,
    IChannelRepository channelRepository,
    UserManager<AppUser> userManager,
    IServerMemberRepository serverMemberRepository
    ) : IRequestHandler<GetChannelSummariesQuery, List<GetChannelSummariesQueryResponse>>
{
    public Task<List<GetChannelSummariesQueryResponse>> Handle(GetChannelSummariesQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            throw new ArgumentNullException(nameof(userId));

        var serverMember = serverMemberRepository.Where(p => p.UserId == userId && p.ServerId == request.serverId && !p.IsDeleted).Include(p => p.ServerMemberRoles).ThenInclude(p => p.AppRole).FirstOrDefault();

        if (serverMember is null || !serverMember.ServerMemberRoles.Any())
        {
            return Task.FromResult(new List<GetChannelSummariesQueryResponse>());
        }

        var channels = channelRepository.Where(c => c.ServerId == request.serverId).ToList();

        if (!channels.Any())
        {
            return Task.FromResult(new List<GetChannelSummariesQueryResponse>());
        }

        List<Channel> channelsInServer = new List<Channel>();

        foreach (var channel in channels)
        {
            var channelRoles = channelRolePermissionRepository.Where(p => p.ChannelId == channel.Id).ToList();
            if (channelRoles.Any(cr => serverMember.ServerMemberRoles.Any(p => p.AppRoleId == cr.RoleId)))
            {
                channelsInServer.Add(channel);
            }
        }

        var response = channelsInServer
                .GroupJoin(userManager.Users,
                    channel => channel.CreateUserId,
                    createUser => createUser.Id,
                    (channel, createUsers) => new { channel, createUsers })
                .SelectMany(
                    cc => cc.createUsers.DefaultIfEmpty(),
                    (cc, createUser) => new { cc.channel, createUser })
                .GroupJoin(userManager.Users,
                    cc => cc.channel.UpdateUserId,
                    updateUser => updateUser.Id,
                    (cc, updateUsers) => new { cc.channel, cc.createUser, updateUsers })
                .SelectMany(
                    cc => cc.updateUsers.DefaultIfEmpty(),
                    (cc, updateUser) => new GetChannelSummariesQueryResponse
                    {
                        Id = cc.channel.Id,
                        Name = cc.channel.Name,
                        Description = cc.channel.Description,
                        IconUrl = cc.channel.IconUrl,
                        ChannelType = cc.channel.Type,
                    }).ToList();

        return Task.FromResult(response);
    }
}
