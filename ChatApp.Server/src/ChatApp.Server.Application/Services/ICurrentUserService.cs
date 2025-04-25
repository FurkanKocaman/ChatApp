namespace ChatApp.Server.Application.Services;
public interface ICurrentUserService
{
    Guid? UserId { get; }
}
