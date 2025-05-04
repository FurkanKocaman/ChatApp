using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Channels;
using MediatR;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelUpdateCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsPublic,
    List<Guid>? RoleIds
    ) : IRequest<Result<string>>;

internal sealed class ChannelUpdateCommandHandler(
    IChannelRepository channelRepository,
    IChannelRolePermissionRepository channelRolePermissionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ChannelUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChannelUpdateCommand request, CancellationToken cancellationToken)
    {
        var channel = await channelRepository.FirstOrDefaultAsync(p => p.Id == request.Id);

        if (channel is null)
            return Result<string>.Failure("Channel not found");

        channel.Name = request.Name;
        channel.Description = request.Description;
        channel.IsPublic = request.IsPublic;

        var existingPermissions = channelRolePermissionRepository
        .Where(p => p.ChannelId == channel.Id)
        .ToList();

        var existingRoleIds = existingPermissions.Select(p => p.RoleId).ToHashSet();
        var requestedRoleIds = request.RoleIds?.ToHashSet() ?? new HashSet<Guid>();

        var toRemove = existingPermissions.Where(p => !requestedRoleIds.Contains(p.RoleId)).ToList();
        channelRolePermissionRepository.DeleteRange(toRemove);

        var toAdd = requestedRoleIds
            .Where(roleId => !existingRoleIds.Contains(roleId))
            .Select(roleId => new ChannelRolePermission
            {
                ChannelId = channel.Id,
                RoleId = roleId,
                CanEdit = true 
            }).ToList();

        await channelRolePermissionRepository.AddRangeAsync(toAdd);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Channel updated");
    }
}
