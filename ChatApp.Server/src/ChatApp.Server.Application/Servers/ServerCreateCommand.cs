using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Servers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Servers;
public sealed record ServerCreateCommand(
    string Name,
    string? Description,
    string? IconUrl
    ) : IRequest<Result<string>>;

internal sealed class ServerCreateCommandHandler(
    ICurrentUserService currentUserService,
    IServerRepository serverRepository,
    IServerMemberRepository serverMemberRepository,
    IServerMemberRoleRepository serverMemberRoleRepository,
    RoleManager<AppRole> roleManager,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ServerCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ServerCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;

                if (!userId.HasValue)
                    return Result<string>.Failure("User not found");

                Domain.Servers.Server server = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    IconUrl = request.IconUrl,
                    OwnerId = userId.Value,
                    CreatedAt = DateTime.Now,
                    CreateUserId = userId.Value,
                };

                serverRepository.Add(server);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                AppRole adminRole = new()
                {
                    Name = "Admin",
                    ServerId = server.Id,
                    CreatedAt = DateTime.Now,
                    CreateUserId = userId.Value,
                };

                AppRole userRole = new()
                {
                    Name = "User",
                    ServerId = server.Id,
                    CreatedAt = DateTime.Now,
                    CreateUserId = userId.Value,
                };

                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(userRole);

                ServerMember member = new()
                {
                    ServerId = server.Id,
                    UserId = userId.Value,
                    CreatedAt = DateTime.Now,
                    CreateUserId = userId.Value,
                };

                serverMemberRepository.Add(member);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                ServerMemberRole serverMemberRole = new()
                {
                    ServerMemberId = member.Id,
                    AppRoleId = adminRole.Id,

                };

                serverMemberRoleRepository.Add(serverMemberRole);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Sever created successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Error : " + ex.Message);
            }
        }
       
    }
}
