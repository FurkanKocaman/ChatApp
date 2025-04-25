using ChatApp.Server.Domain.Channels;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;

internal sealed class ChannelRepository : Repository<Channel, ApplicationDbContext>, IChannelRepository
{
    public ChannelRepository(ApplicationDbContext context) : base(context)
    {
    }
}