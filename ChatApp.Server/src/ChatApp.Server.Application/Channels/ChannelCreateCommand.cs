using ChatApp.Server.Domain.Channels;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Channels;

public sealed record ChannelCreateCommand(
    string Name,
    string? Description,
    string? ImageUrl,
    bool IsPublic) : IRequest<Result<string>>;

internal sealed class ChannelCreateCommandHandler(
    IChannelRepository channelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChannelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChannelCreateCommand request, CancellationToken cancellationToken)
    {
        var isChannelExist = await channelRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
        if (isChannelExist)
            return Result<string>.Failure("This channel name is already taken");

        Channel channel = request.Adapt<Channel>();
        channelRepository.Add(channel);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed($"{channel.Name} is created.");
    }
}
