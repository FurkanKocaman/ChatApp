using ChatApp.Server.Domain.DTOs;
using ChatApp.Server.Domain.ServerMembers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Application.ServerMembers;
public sealed record GetServerMemberSummariesQuery(
    Guid serverId,
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetServerMemberSummariesQueryResponse>>;

public sealed class GetServerMemberSummariesQueryResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public List<string> Roles { get; set; } = new List<string>();
    public string Status { get; set; } = default!;
    public DateTimeOffset? JoinedAt { get; set; }
}

internal sealed class GetServerMemberSummariesQueryHandler(
    IServerMemberRepository serverMemberRepository
    ) : IRequestHandler<GetServerMemberSummariesQuery, PagedResult<GetServerMemberSummariesQueryResponse>>
{
    public Task<PagedResult<GetServerMemberSummariesQueryResponse>> Handle(GetServerMemberSummariesQuery request, CancellationToken cancellationToken)
    {
        var serverMembers = serverMemberRepository.Where(p => p.ServerId == request.serverId && !p.IsDeleted)
            .Include(p => p.ServerMemberRoles).ThenInclude(p => p.AppRole)
            .Include(p => p.User)
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var totalCount = serverMembers.Count();

        var response = serverMembers.Select(p => new GetServerMemberSummariesQueryResponse
        {
            Id = p.UserId,
            FullName = p.User != null ? p.User.FullName : "null",
            Email = p.User != null ? p.User.Email != null ? p.User.Email : "null" : "null",
            Roles = p.ServerMemberRoles.Select(p => p.AppRole.Name).ToList()!,
            Status = "Active",
            JoinedAt = p.CreatedAt,
        });

        return Task.FromResult(new PagedResult<GetServerMemberSummariesQueryResponse>(response, request.page, request.pageSize, totalCount));




    }
}