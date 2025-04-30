using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Roles;
using ChatApp.Server.Domain.ServerMembers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace ChatApp.Server.Infrastructure.Services;
internal sealed class PermissionCacheService : IPermissionCacheService
{
    private readonly StackExchange.Redis.IDatabase _redisDb;
    private readonly TimeSpan _cacheTimeout;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IServerMemberRepository _serverMemberRepository;

    public PermissionCacheService(IConnectionMultiplexer redis, IConfiguration configuration, RoleManager<AppRole> roleManager, IServerMemberRepository serverMemberRepository)
    {
        _redisDb = redis.GetDatabase();
        _cacheTimeout = configuration.GetValue<TimeSpan>("Redis:CacheTimeout");
        _roleManager = roleManager;
        _serverMemberRepository = serverMemberRepository;
    }
    public async Task<IEnumerable<string>> GetPermissionsAsync(Guid userId, Guid ServerId)
    {
        var cacheKey = GetCacheKey(userId, ServerId);

        var cachePermissions = await _redisDb.StringGetAsync(cacheKey);

        if (!cachePermissions.IsNullOrEmpty)
            return JsonSerializer.Deserialize<List<string>>(cachePermissions!)!;

        var permissions = await FetchFromDatabase(userId, ServerId);

        await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(permissions),_cacheTimeout);

        return permissions;

    }

    private static string GetCacheKey(Guid userId, Guid serverId) 
        => $"permissions:{userId}:{serverId}";

    private async Task<List<string>> FetchFromDatabase(
        Guid userId,
        Guid serverId)
    {
        var serverMember = await _serverMemberRepository.Where(p => p.UserId == userId && p.ServerId == serverId && !p.IsDeleted).Include(p => p.ServerMemberRoles).ThenInclude(p => p.AppRole).FirstOrDefaultAsync();

        if (serverMember is null)
            return new List<string>();

        var roles = serverMember.ServerMemberRoles;

        if (roles is null || !roles.Any())
            return new List<string>();
        var permissions = new List<string>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role.AppRole);
            permissions.AddRange(claims.Select(p => p.Value));
        }

        return permissions;

    }
}
