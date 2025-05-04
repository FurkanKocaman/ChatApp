using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ChannelRolePermissions;
using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.Servers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Channels;
public sealed record ChannelCreateCommand(
    Guid ServerId,
    string Name,
    string? Description,
    string? IconUrl,
    bool IsPublic,
    List<Guid>? RoleIds,
    int ChannelType
    ) : IRequest<Result<string>>;

internal sealed class ChannelCreateCommandHandler(
    IChannelRepository channelRepository,
    ICurrentUserService currentUserService,
    IServerRepository serverRepository,
    RoleManager<AppRole> roleManager,
    IChannelRolePermissionRepository channelRolePermissionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ChannelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChannelCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;

                if (!userId.HasValue)
                    return Result<string>.Failure("User not found");

                var serverExist = await serverRepository.AnyAsync(s => s.Id == request.ServerId);
                if (!serverExist)
                    return Result<string>.Failure("Server not found");

                Channel channel = request.Adapt<Channel>();

                channelRepository.Add(channel);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                if (!channel.IsPublic)
                {
                    if (request.RoleIds is null || !request.RoleIds.Any())
                    {
                        await unitOfWork.RollbackTransactionAsync(transaction);
                        return Result<string>.Failure("There should be atleast one selected role for private channel");
                    }
                    foreach (var roleId in request.RoleIds)
                    {
                        if (roleManager.Roles.Any(r => r.Id == roleId))
                        {
                            ChannelRolePermission permission = new()
                            {
                                RoleId = roleId,
                                ChannelId = channel.Id,
                            };
                            channelRolePermissionRepository.Add(permission);
                        }
                    }

                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("Channel created successfully");
            }catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("There has been error : "+ex);
            }
        }
       
    }
}
