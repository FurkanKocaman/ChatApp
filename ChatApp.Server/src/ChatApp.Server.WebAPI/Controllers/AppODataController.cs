using ChatApp.Server.Application.Channels;
using ChatApp.Server.Application.Messages;
using ChatApp.Server.Application.Roles;
using ChatApp.Server.Application.ServerMembers;
using ChatApp.Server.Application.Servers;
using ChatApp.Server.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonelYonetim.Server.Domain.RoleClaim;

namespace ChatApp.Server.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();

        builder.EntitySet<GetUserJoinedServersQueryResponse>("user-servers");
        builder.EntitySet<MessagesGetAllQueryResponse>("messages");
        builder.EntitySet<ServerMembersGetAllQueryResponse>("server-members");
        builder.EntitySet<GetChannelsInServerQueryResponse>("server-channels");
        builder.EntitySet<ChannelGetDetailsQueryResponse>("channel-details");
        builder.EntitySet<GetRoleDetailsByServerQueryResponse>("roles");
        builder.EntitySet<GetUserModeratedServersQueryResponse>("servers-moderated");

        return builder.GetEdmModel();
    }

    [HttpGet("users")]
    public async Task<IQueryable<UserGetAllQueryResponse>> GetUsers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("user-current")]
    [Authorize()]
    public async Task<UserGetCurrentQueryResponse> GetCurrentUser(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetCurrentQuery(), cancellationToken);
        return response;
    }

    [HttpGet("user-servers")]
    [Authorize()]
    //[Authorize(Policy = Permissions.CreateChannel)]
    public async Task<IQueryable<GetUserJoinedServersQueryResponse>> GetUserJoinedServers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetUserJoinedServersQuery(), cancellationToken);
        return response;
    }

    [HttpGet("messages/{channelId}")]
    [Authorize()]
    public async Task<IQueryable<MessagesGetAllQueryResponse>> GetMessages(Guid channelId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new MessagesGetAllQuery(channelId), cancellationToken); 
        return response;
    }

    [HttpGet("server-channels/{serverId}")]
    [Authorize()]
    public async Task<IQueryable<GetChannelsInServerQueryResponse>> GetChannels(Guid serverId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetChannelsInServerQuery(serverId), cancellationToken);
        return response;
    }

    [HttpGet("server-members/{serverId}")]
    [Authorize()]
    public async Task<IQueryable<ServerMembersGetAllQueryResponse>> GetServerMembers(Guid serverId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new ServerMembersGetAllQuery(serverId), cancellationToken);
        return response;
    }

    [HttpGet("channel-details/{serverId}")]
    [Authorize()]
    public async Task<IQueryable<ChannelGetDetailsQueryResponse>> GetChannelDetails(Guid serverId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new ChannelGetDetailsQuery(serverId), cancellationToken);
        return response;
    }

    [HttpGet("roles/{serverId}")]
    [Authorize()]
    public async Task<IQueryable<GetRoleDetailsByServerQueryResponse>> GetRoleDetails(Guid serverId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetRoleDetailsByServerQuery(serverId), cancellationToken);
        return response;
    }

    [HttpGet("servers-moderated")]
    [Authorize()]
    public async Task<IQueryable<GetUserModeratedServersQueryResponse>> GetServersModeratedByUser(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetUserModeratedServersQuery(), cancellationToken);
        return response;
    }
}

