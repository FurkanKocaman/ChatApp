using ChatApp.Server.Domain.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ChatApp.Server.Application.Tokens;
public sealed record ValidateServerInvitationTokenCommand(
    string token
    ) : IRequest<Result<InviteValidationResponse>>;

public sealed class InviteValidationResponse
{
    public bool IsValid { get; set; }
    public string Status { get; set; } = default!;
    public string ServerName { get; set; } = default!;
    public string InviterName { get; set; } = default!;
    public Guid ServerId { get; set; } = default!;
}

internal sealed class ValidateServerInvitationTokenCommandHandler(
    ITokenRepository tokenRepository
    ) : IRequestHandler<ValidateServerInvitationTokenCommand, Result<InviteValidationResponse>>
{
    public async Task<Result<InviteValidationResponse>> Handle(ValidateServerInvitationTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenRepository.Where(p => p.Data == request.token).Include(p => p.Server).Include(p => p.Creator).FirstOrDefaultAsync(cancellationToken);

        if (token is null)
            return Result<InviteValidationResponse>.Failure("Token not found");

        var response = new InviteValidationResponse()
        {
            IsValid = token.ExpirationDate < DateTimeOffset.Now ? false: token.CurrentUsageCount < token.MaxUsageCount ? true: false,
            Status = token.ExpirationDate < DateTimeOffset.Now ? "Token has expired" : token.CurrentUsageCount < token.MaxUsageCount ? "Token is avaliable to use" : "Token was already used",
            ServerName = token.Server!.Name,
            InviterName = token.Creator!.FullName,
            ServerId = token.ServerId,
        };

        return Result<InviteValidationResponse>.Succeed(response);
    }
}
