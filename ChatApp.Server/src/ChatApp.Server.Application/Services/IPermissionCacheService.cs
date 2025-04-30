namespace ChatApp.Server.Application.Services;
public interface IPermissionCacheService
{
    Task<IEnumerable<string>> GetPermissionsAsync(Guid userId, Guid ServerId);

}
