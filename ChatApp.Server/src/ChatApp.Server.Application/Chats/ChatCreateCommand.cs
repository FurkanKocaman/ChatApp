using ChatApp.Server.Domain.Chats;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Chats;

public sealed record ChatCreateCommand(
    string Name,
    string? Description,
    Guid ChannelId) : IRequest<Result<string>>;

internal sealed class ChatCreateCommandHandler(
    IChatRepository chatRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChatCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChatCreateCommand request, CancellationToken cancellationToken)
    {
        var isChatExist = await chatRepository.AnyAsync(c => c.Name == request.Name);
        if (isChatExist)
            return Result<string>.Failure($"Chat named to {request.Name} is already exist");

        Chat chat = request.Adapt<Chat>();
        chatRepository.Add(chat);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed($"{chat.Name} is created.");
    }
}
