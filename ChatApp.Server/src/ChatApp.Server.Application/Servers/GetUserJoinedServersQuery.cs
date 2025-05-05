using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.ServerMembers;
using ChatApp.Server.Domain.Servers;
using MediatR;

namespace ChatApp.Server.Application.Servers;
public sealed record GetUserJoinedServersQuery(
    ) : IRequest<List<GetUserJoinedServersQueryResponse>>;


public sealed class GetUserJoinedServersQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
}

internal sealed class GetUserJoinedServersQueryHandler(
    ICurrentUserService currentUserService,
    IServerRepository serverRepository,
    IServerMemberRepository serverMemberRepository
    ) : IRequestHandler<GetUserJoinedServersQuery, List<GetUserJoinedServersQueryResponse>>
{
    public Task<List<GetUserJoinedServersQueryResponse>> Handle(GetUserJoinedServersQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue)
            return Task.FromResult(new List<GetUserJoinedServersQueryResponse>()); 

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
                }).ToList();

        return Task.FromResult(response);
    }
}
