using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelGetDetailsQuery(
    Guid serverId
    ) : IRequest<IQueryable<ChannelGetDetailsQueryResponse>>;

public sealed class ChannelGetDetailsQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string Access { get; set; } = default!;
    public int MessageCount { get; set; }
    public string MostActiveUser { get; set; } = default!;
    public string Status { get; set; } = default!;
}

internal sealed class ChannelGetDetailsQueryHandler(
    IChannelRepository channelRepository,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    //,IMessageRepository messageRepository
    ) : IRequestHandler<ChannelGetDetailsQuery, IQueryable<ChannelGetDetailsQueryResponse>>
{
    public Task<IQueryable<ChannelGetDetailsQueryResponse>> Handle(ChannelGetDetailsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Task.FromResult(Enumerable.Empty<ChannelGetDetailsQueryResponse>().AsQueryable());

        var channels = channelRepository.Where(p => p.ServerId == request.serverId);

        var x = channels.ToList();

        var response = channels
            .Join(userManager.Users,
            channel => channel.CreateUserId,
            createUser => createUser.Id,
            (channel, createUser) => new {channel, createUser})
           .Select(c => new ChannelGetDetailsQueryResponse
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
               CreateUserName = c.createUser.FullName + "("+c.createUser.Email +")",
               UpdateAt = c.channel.UpdateAt,
               UpdateUserId = c.channel.UpdateUserId,
               UpdateUserName = "",
               IsDeleted = c.channel.IsDeleted,
               DeleteAt = c.channel.DeleteAt,
           });

        return Task.FromResult(response);
    }
}
