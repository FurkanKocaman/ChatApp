using ChatApp.Server.Domain.Tokens;
using ChatApp.Server.Infrastructure.Context;
using GenericRepository;

namespace ChatApp.Server.Infrastructure.Repositories;
internal sealed class TokenRepository : Repository<Token, ApplicationDbContext>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
