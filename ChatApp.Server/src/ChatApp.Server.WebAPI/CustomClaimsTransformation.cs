using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace PersonelYonetim.Server.WebAPI;

public class CustomClaimsTransformation(RoleManager<AppRole> roleManager, IHttpContextAccessor httpContextAccessor, IPermissionCacheService permissionCacheService) : IClaimsTransformation
{
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions());
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var serverId = httpContextAccessor.HttpContext?.Request.Headers["X-Current-Server"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(serverId))
            return principal;

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier);

        if(string.IsNullOrEmpty(userId.Value))
            return principal;

        var permissions = await permissionCacheService.GetPermissionsAsync(Guid.Parse(userId.Value), Guid.Parse(serverId));

        var identity = principal.Identity as ClaimsIdentity;
        if(identity == null)
            return principal;

        foreach (var claim in identity.FindAll("permission").ToList())
        {
            identity.RemoveClaim(claim);
        }

        foreach (var permission in permissions)
        {
            identity.AddClaim(new Claim("permission", permission));
        }

        identity.AddClaim(new Claim("current_server", serverId.ToString()));

        return principal;
    }
}
