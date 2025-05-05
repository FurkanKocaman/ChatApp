namespace ChatApp.Server.Domain.DTOs;
public sealed record PagedResult<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    int TotalCount);
