using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ServerMembers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Application.ServerMembers;
public sealed record ServerMembersGetAllQuery(
    Guid serverId
    ) : IRequest<IQueryable<ServerMembersGetAllQueryResponse>>;

public sealed class ServerMembersGetAllQueryResponse
{
    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
}

internal sealed class ServerMembersGetAllQueryHandler(
    IServerMemberRepository serverMemberRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<ServerMembersGetAllQuery, IQueryable<ServerMembersGetAllQueryResponse>>
{
    public Task<IQueryable<ServerMembersGetAllQueryResponse>> Handle(ServerMembersGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Task.FromResult(Enumerable.Empty<ServerMembersGetAllQueryResponse>().AsQueryable());

        var serverMembers = serverMemberRepository.Where(p => p.ServerId == request.serverId).Include(p => p.User);

        var response = serverMembers
                .Select(s => new ServerMembersGetAllQueryResponse
                {
                    Id = s.UserId,
                    ServerId = s.ServerId,
                    UserId = s.UserId,
                    FullName = s.User!= null ? s.User.FullName : "null",
                    DisplayName = s.User != null ? s.User.DisplayName : null,
                    AvatarUrl = s.User != null ? s.User.AvatarUrl : null,
                }).AsQueryable();

        return Task.FromResult(response);
    }
}
