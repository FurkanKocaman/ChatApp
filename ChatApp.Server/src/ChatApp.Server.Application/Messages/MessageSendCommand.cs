using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Messages;
public sealed record MessageSendCommand(
    Guid ChannelId,
    string Content,
    int Type
    ) : IRequest<Result<string>>;

internal sealed class MessageSendCommandHandler(
    ICurrentUserService currentUserService,
    IMessageRepository messageRepository,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    IChatHubService chatHubService
    ) : IRequestHandler<MessageSendCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MessageSendCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            return Result<string>.Failure("User not found");

        Message message = request.Adapt<Message>();

        messageRepository.Add( message );
        await unitOfWork.SaveChangesAsync(cancellationToken);

        MessagesGetAllQueryResponse res = new()
        {
            Id = message.Id,
            ChannelId = message.ChannelId,
            Content = message.Content,
            Type = message.Type,
            ImageUrl = message.ImageUrl,
            FileUrl = message.FileUrl,
            FileName = message.FileName,
            FileSize = message.FileSize,
            SenderName = user.FullName,
            SendDate= DateTimeOffset.Now,
        };

        await chatHubService.SendMessageToChannelAsync(message.ChannelId.ToString(), res);

        return Result<string>.Succeed("message sended");
    }
}
