using Auth.Domain.Entities.RefreshTokens;
using Auth.Persistence.DBContext;
using Auth.Persistence.Repositories;

namespace Auth.Persistence.UnitOfWork;

public partial class UnitOfWork(EFContext eFContext)
{
    private readonly EFContext _eFContext = eFContext;

    public IRefreshTokenRepository RefreshToken { get; } = new RefreshTokenRepository(eFContext);
}
