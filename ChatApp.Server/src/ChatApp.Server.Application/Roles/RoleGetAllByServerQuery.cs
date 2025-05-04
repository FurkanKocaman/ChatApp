using ChatApp.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Server.Application.Roles;
public sealed record RoleGetAllByServerQuery(
    Guid serverId
    ) : IRequest<List<RoleGetAllByServerQueryResponse>>;

public sealed class RoleGetAllByServerQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Level { get; set; }
}

internal sealed class RoleGetAllByServerQueryHandler(
    RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleGetAllByServerQuery, List<RoleGetAllByServerQueryResponse>>
{
    public Task<List<RoleGetAllByServerQueryResponse>> Handle(RoleGetAllByServerQuery request, CancellationToken cancellationToken)
    {
        var roles = roleManager.Roles.Where(p => p.ServerId == request.serverId && !p.IsDeleted).ToList();

        var response = roles.Select(r => new RoleGetAllByServerQueryResponse
        {
            Id = r.Id,
            Name = r.Name ?? "Null",
            Level = r.Level,
        }).OrderBy(p => p.Level).ToList();

        return Task.FromResult(response);
    }
}
