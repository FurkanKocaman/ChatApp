using ChatApp.Server.Application.Channels;
using ChatApp.Server.Application.Messages;
using ChatApp.Server.Application.Roles;
using ChatApp.Server.Application.ServerMembers;
using ChatApp.Server.Application.Servers;
using ChatApp.Server.Application.Users;
using ChatApp.Server.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace ChatApp.Server.WebAPI.Controllers.v1;
[Route("api/v1")]
[ApiController]
public class AppController(ISender sender) : ControllerBase
{
    //User

    [HttpGet("user/current")]
    [Authorize()]
    public async Task<ActionResult<UserGetCurrentQueryResponse>> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new UserGetCurrentQuery(), cancellationToken);
        return Ok(response);
    }

    //Servers

    [HttpGet("servers/moderated")]
    [Authorize()]
    public async Task<ActionResult<GetUserModeratedServersQueryResponse>> GetModeratedServers(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUserModeratedServersQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("servers")]
    [Authorize()]
    public async Task<ActionResult<GetUserJoinedServersQueryResponse>> GetServers(CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new GetUserJoinedServersQuery(),cancellationToken);
        return Ok(response);
    }

    //Channels

    [HttpGet("servers/{serverId}/channels")]
    public async Task<IActionResult> GetChannels(
    Guid serverId,
    [FromQuery] string view = "summaries",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        if (view.Equals("summaries", StringComparison.OrdinalIgnoreCase))
        {
            var summaries = await sender.Send(new GetChannelSummariesQuery(serverId, page,pageSize), cancellationToken);
            return Ok(summaries);
        }
        if (view.Equals("details", StringComparison.OrdinalIgnoreCase))
        {
            var details = await sender.Send(new GetChannelDetailsQuery(serverId, page, pageSize), cancellationToken);
            return Ok(details);
        }
        return BadRequest("Invalid view parameter");
    }

    [HttpGet("channels/{id}")]
    [Authorize(Permissions.ViewChannel)]

    public async Task<Result<ChannelGetQueryResponse>> GetChannel(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new ChannelGetQuery(id), cancellationToken);
        return response;
    }

    //Messages
    [HttpGet("messages/{channelId}")]
    [Authorize()]
    public async Task<ActionResult<PagedResult<MessagesGetAllQueryResponse>>> GetMessagesByChannel(
        Guid channelId, 
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20, 
        CancellationToken cancellationToken = default)
    {
        var response = await sender.Send(new MessagesGetAllQuery(channelId,page,pageSize), cancellationToken);
        return Ok(response);
    }

    //Roles

    [HttpGet("servers/{serverId}/roles")]
    [Authorize()]
    public async Task<IQueryable<GetRoleDetailsByServerQueryResponse>> GetRoleDetails(Guid serverId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetRoleDetailsByServerQuery(serverId), cancellationToken);
        return response;
    }

    //Members

    [HttpGet("servers/{serverId}/members")]
    public async Task<IActionResult> GetMembers(
    Guid serverId,
    [FromQuery] string view = "summaries",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        if (view.Equals("summaries", StringComparison.OrdinalIgnoreCase))
        {
            var summaries = await sender.Send(new GetServerMemberSummariesQuery(serverId, page, pageSize), cancellationToken);
            return Ok(summaries);
        }
        if (view.Equals("details", StringComparison.OrdinalIgnoreCase))
        {
            var details = await sender.Send(new GetServerMemberDetailsQuery(serverId, page, pageSize), cancellationToken);
            return Ok(details);
        }
        return BadRequest("Invalid view parameter");
    }


}
