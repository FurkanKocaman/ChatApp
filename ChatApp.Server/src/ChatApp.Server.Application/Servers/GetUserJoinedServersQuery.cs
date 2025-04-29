using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Servers;
using MediatR;

namespace ChatApp.Server.Application.Servers;
public sealed record GetUserJoinedServersQuery(
    ) : IRequest<IQueryable<GetUserJoinedServersQueryResponse>>;


public sealed class GetUserJoinedServersQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    //public DateTimeOffset? JoinedAt { get; set; }
}

internal sealed class GetUserJoinedServersQueryHandler(
    ICurrentUserService currentUserService,
    IServerRepository serverRepository,
    IServerMemberRepository serverMemberRepository
    ) : IRequestHandler<GetUserJoinedServersQuery, IQueryable<GetUserJoinedServersQueryResponse>>
{
    public Task<IQueryable<GetUserJoinedServersQueryResponse>> Handle(GetUserJoinedServersQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            throw new ArgumentNullException(nameof(userId));

        var serverMembers = serverMemberRepository.Where(p => p.UserId == userId);

        var response = serverRepository.GetAll()
                .Join(serverMembers,
                    server => server.Id,
                    serverMember => serverMember.ServerId,
                    (server, serverMember) => new {server, serverMember})
                .Select(ss => new GetUserJoinedServersQueryResponse
                {
                    Id = ss.server.Id,
                    Name = ss.server.Name,
                    IconUrl = ss.server.IconUrl,
                }).AsQueryable();

        return Task.FromResult(response);
    }
}
