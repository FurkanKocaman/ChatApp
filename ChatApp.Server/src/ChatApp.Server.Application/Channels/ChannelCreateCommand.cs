using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Servers;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelCreateCommand(
    Guid ServerId,
    string Name,
    string? Description,
    string? IconUrl,
    int ChannelType
    ) : IRequest<Result<string>>;

internal sealed class ChannelCreateCommandHandler(
    IChannelRepository channelRepository,
    ICurrentUserService currentUserService,
    IServerRepository serverRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ChannelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChannelCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var serverExist = await serverRepository.AnyAsync(s => s.Id == request.ServerId);
        if (!serverExist)
            return Result<string>.Failure("Server not found");

        Channel channel = request.Adapt<Channel>();

        channelRepository.Add( channel );
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Channel created successfully");
    }
}
