using ChatApp.Server.Domain.UserRoles;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class UserRoleRepository : Repository<UserRole, ApplicationDbContext>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
