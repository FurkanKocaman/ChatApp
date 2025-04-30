using ChatApp.Server.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ChatApp.Server.Infrastructure.Services;
internal sealed class CurrentUserService (
    IHttpContextAccessor httpContextAccessor
    ) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return null;
            }
            return Guid.Parse(userIdString);
        }
    }
    public Guid? ServerId
    {
        get
        {
            var serverId = httpContextAccessor.HttpContext?.Request.Headers["X-Current-Server"];
            if (string.IsNullOrEmpty(serverId))
            {
                return null;
            }
            return Guid.Parse(serverId!);
        }
    }

}
