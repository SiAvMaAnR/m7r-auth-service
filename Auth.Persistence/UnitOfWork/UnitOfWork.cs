using Auth.Domain.Common;

namespace Auth.Persistence.UnitOfWork;

public partial class UnitOfWork : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await _eFContext.SaveChangesAsync();
    }

    public void SaveChanges()
    {
        _eFContext.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _eFContext.Dispose();
    }
}
