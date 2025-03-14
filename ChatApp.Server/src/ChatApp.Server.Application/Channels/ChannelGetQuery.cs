using ChatApp.Server.Domain.Channels;
using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Channels;

public sealed record ChannelGetQuery(Guid Id) : IRequest<Result<Channel>>;

internal sealed class ChannelGetQueryHandler(
    IChannelRepository channelRepository) : IRequestHandler<ChannelGetQuery, Result<Channel>>
{
    public async Task<Result<Channel>> Handle(ChannelGetQuery request, CancellationToken cancellationToken)
    {
        Channel channel = await channelRepository.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (channel is null)
            return Result<Channel>.Failure("Channel not found.");

        return Result<Channel>.Succeed(channel);
    }
}


