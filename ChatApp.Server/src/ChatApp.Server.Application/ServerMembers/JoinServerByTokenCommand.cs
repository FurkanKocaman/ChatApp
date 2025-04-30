using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMemberRoles;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.ServerMembers;
public sealed record JoinServerByTokenCommand(
    string token
    ) : IRequest<Result<string>>;

internal sealed class JoinServerByTokenCommandHandler(
    ITokenRepository tokenRepository,
    ICurrentUserService currentUserService,
    IServerMemberRepository serverMemberRepository,
    IServerMemberRoleRepository serverMemberRoleRepository,
    RoleManager<AppRole> roleManager,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<JoinServerByTokenCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JoinServerByTokenCommand request, CancellationToken cancellationToken)
    {
       using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                if (!userId.HasValue)
                    return Result<string>.Failure("User not found");

                var token = await tokenRepository.WhereWithTracking(p => p.Data == request.token ).FirstOrDefaultAsync();

                if (token is null)
                    return Result<string>.Failure("token not found or used");

                if (token.ExpirationDate < DateTimeOffset.Now)
                    return Result<string>.Failure("Token expired");

                if (token.CurrentUsageCount >= token.MaxUsageCount)
                    return Result<string>.Failure("Token was used by someone");

                token.CurrentUsageCount += 1;

                var role = await roleManager.Roles.Where(p => p.ServerId == token.ServerId).OrderByDescending(p => p.Level).FirstOrDefaultAsync();

                if (role is null)
                    return Result<string>.Failure("Joining server failed");

                var isServerMemberExist = await serverMemberRepository.AnyAsync(p => p.ServerId == token.ServerId && p.UserId == userId.Value && !p.IsDeleted);

                if (isServerMemberExist)
                    return Result<string>.Failure("You are already member of this server");

                ServerMember serverMember = new()
                {
                    ServerId = token.ServerId,
                    UserId = userId.Value,
                    CreatedAt = DateTimeOffset.Now,
                    CreateUserId = userId.Value,
                };

                serverMemberRepository.Add(serverMember);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                ServerMemberRole serverMemberRole = new()
                {
                    ServerMemberId = serverMember.Id,
                    AppRoleId = role.Id,
                };

                serverMemberRoleRepository.Add(serverMemberRole);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Joined server successfully");
            }catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure(ex.ToString());
            }
        }

    }
}
