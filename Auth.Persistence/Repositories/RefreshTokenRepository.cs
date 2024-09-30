using Auth.Domain.Entities.RefreshTokens;
using Auth.Persistence.DBContext;
using Auth.Persistence.Repositories.Common;

namespace Auth.Persistence.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(EFContext dbContext)
        : base(dbContext) { }
}
