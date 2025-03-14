using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default);
}
