using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Messages;
public sealed record MessagesGetAllQuery(
    Guid ChannelId
    ) : IRequest<IQueryable<MessagesGetAllQueryResponse>>;

public sealed class MessagesGetAllQueryResponse
{
    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Text;
    public string? ImageUrl { get;  set; }
    public string? FileUrl { get;  set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string SenderName { get; set; } = default!;
    public DateTimeOffset SendDate { get; set; }
}

internal sealed class MessagesGetAllQueryHandler(
    IMessageRepository messageRepository,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<MessagesGetAllQuery, IQueryable<MessagesGetAllQueryResponse>>
{
    public Task<IQueryable<MessagesGetAllQueryResponse>> Handle(MessagesGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        
        if(!userId.HasValue)
            return Task.FromResult(Enumerable.Empty<MessagesGetAllQueryResponse>().AsQueryable());

        var messages = messageRepository.Where(p => p.ChannelId == request.ChannelId)
            .GroupJoin(userManager.Users,
                message => message.CreateUserId,
                createUsers => createUsers.Id,
                (message, createUsers) => new { message, createUsers })
            .SelectMany(
                uc => uc.createUsers.DefaultIfEmpty(),
                (uc, createUser) => new MessagesGetAllQueryResponse
                {
                    Id = uc.message.Id,
                    ChannelId = uc.message.ChannelId,
                    Content = uc.message.Content,
                    Type = uc.message.Type,
                    ImageUrl = uc.message.ImageUrl,
                    FileUrl = uc.message.FileUrl,
                    FileName = uc.message.FileName,
                    FileSize = uc.message.FileSize,
                    SenderName = createUser != null ? createUser.FullName : "null",
                    SendDate = uc.message.CreatedAt,
                }).OrderBy(p => p.SendDate).AsQueryable();




        return Task.FromResult(messages);
    }
}


