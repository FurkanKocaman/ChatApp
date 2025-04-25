using ChatApp.Server.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ChatApp.Server.Infrastructure.Services;
public sealed class CurrentUserService (
    HttpContextAccessor httpContextAccessor
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

}
