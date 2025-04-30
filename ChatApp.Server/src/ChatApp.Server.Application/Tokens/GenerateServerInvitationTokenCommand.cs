using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Tokens;
using MediatR;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace ChatApp.Server.Application.Tokens;
public sealed record GenerateServerInvitationTokenCommand(
    int count,
    Guid serverId
    ) : IRequest<Result<string>>;

internal sealed class GenerateServerInvitationTokenCommandHandler(
    ITokenRepository tokenRepository,
    ICurrentUserService currentUserService,
    IServerMemberRepository serverMemberRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<GenerateServerInvitationTokenCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GenerateServerInvitationTokenCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var isMemberOfServer = await serverMemberRepository.AnyAsync(p => p.ServerId == request.serverId && p.UserId == userId.Value && !p.IsDeleted);
        if (!isMemberOfServer)
            return Result<string>.Failure("You are not a member of server");

        Token token = new()
        {
            ExpirationDate = DateTimeOffset.Now.AddMinutes(30),
            CreatorId = userId.Value,
            ServerId = request.serverId,
            IsUsed = false,
            TokenType = TokenType.Invitation,
            MaxUsageCount = request.count,
            CurrentUsageCount = 0,
        };

        tokenRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(token.Data);
    }
}
