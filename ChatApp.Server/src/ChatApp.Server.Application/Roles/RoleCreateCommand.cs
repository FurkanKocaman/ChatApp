using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Roles;
public sealed record RoleCreateCommand(
    string Name,
    Guid ServerId,
    string? ColorHex,
    List<string> Claims
    ) : IRequest<Result<string>>;

internal sealed class RoleCreateCommandHandler(
    ) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
