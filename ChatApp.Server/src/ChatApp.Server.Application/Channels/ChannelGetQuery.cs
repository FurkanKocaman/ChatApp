using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Abstractions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelGetQuery(
    Guid ServerId
    ) : IRequest<List<ChannelGetQueryResponse>>;

public sealed class ChannelGetQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public ChannelType ChannelType { get; set; }
}

internal sealed class ChannelGetQueryHandler(
    ICurrentUserService currentUserService,
    IChannelRepository channelRepository,
    IServerMemberRepository serverMemberRepository,
    UserManager<AppUser> userManager
    ) : IRequestHandler<ChannelGetQuery, List<ChannelGetQueryResponse>>
{
    public Task<List<ChannelGetQueryResponse>> Handle(ChannelGetQuery request, CancellationToken cancellationToken)
    {
       Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            throw new ArgumentNullException(nameof(userId));

        var isUserMemberOfServer = serverMemberRepository.Any(p => p.UserId == userId && p.ServerId == request.ServerId && !p.IsDeleted);
        if (!isUserMemberOfServer)
            throw new ArgumentException("User is not a member of server");

        var channels = channelRepository.Where(p => p.ServerId == request.ServerId && !p.IsDeleted);

        var response = channels
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
                    }).ToList();

        return Task.FromResult(response);


    }
}

