using ChatApp.Server.Domain.FriendShips;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class FriendshipRepository: Repository<FriendShip, ApplicationDbContext>, IFriendshipRepository
{
    public FriendshipRepository(ApplicationDbContext context) : base(context)
    {
    }
}
