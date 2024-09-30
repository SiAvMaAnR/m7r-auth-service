using Auth.Domain.Entities;

namespace Auth.Domain.Specification;

public class DefaultSpec<TEntity> : Specification<TEntity>
    where TEntity : BaseEntity
{
    public DefaultSpec()
    {
        ApplyTracking();
    }
}
