using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.DTOs;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace ChatApp.Server.Application.Channels;

public sealed record GetChannelDetailsQuery(
    Guid serverId,
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetChannelDetailsQueryResponse>>;

public sealed class GetChannelDetailsQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string Access { get; set; } = default!;
    public int MessageCount { get; set; }
    public string MostActiveUser { get; set; } = default!;
    public string Status { get; set; } = default!;
}

internal sealed class GetChannelDetailsQueryHandler(
    IChannelRepository channelRepository,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<GetChannelDetailsQuery, PagedResult<GetChannelDetailsQueryResponse>>
{
    public Task<PagedResult<GetChannelDetailsQueryResponse>> Handle(GetChannelDetailsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            throw new ArgumentNullException(nameof(userId));

        var query = channelRepository.Where(p => p.ServerId == request.serverId);

        var totalCount = query.Count();

        var channels = query
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList(); ;

        var response = channels
            .Join(userManager.Users,
            channel => channel.CreateUserId,
            createUser => createUser.Id,
            (channel, createUser) => new { channel, createUser })
           .Select(c => new GetChannelDetailsQueryResponse
           {
               Id = c.channel.Id,
               Name = c.channel.Name,
               Access = "Everyone",
               MessageCount = 1,
               MostActiveUser = "",
               Status = c.channel.IsActive ? "Active" : "Passive",
               IsActive = c.channel.IsActive,
               CreatedAt = c.channel.CreatedAt,
               CreateUserId = c.channel.CreateUserId,
               CreateUserName = c.createUser.FullName + "(" + c.createUser.Email + ")",
               UpdateAt = c.channel.UpdateAt,
               UpdateUserId = c.channel.UpdateUserId,
               UpdateUserName = "",
               IsDeleted = c.channel.IsDeleted,
               DeleteAt = c.channel.DeleteAt,
           });   

        return Task.FromResult(new PagedResult<GetChannelDetailsQueryResponse>(response,request.page, request.pageSize,totalCount));
    }
}
