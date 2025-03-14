
using ChatApp.Server.Domain.Chats;
using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Chats;

public sealed record ChatGetQuery(Guid Id) : IRequest<Result<Chat>>;

internal sealed class ChatGetQueryHandler(
    IChatRepository chatRepository) : IRequestHandler<ChatGetQuery, Result<Chat>>
{
    public async Task<Result<Chat>> Handle(ChatGetQuery request, CancellationToken cancellationToken)
    {
        Chat chat= await chatRepository.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (chat is null)
            return Result<Chat>.Failure("Channel not found.");

        return Result<Chat>.Succeed(chat);
    }
}
