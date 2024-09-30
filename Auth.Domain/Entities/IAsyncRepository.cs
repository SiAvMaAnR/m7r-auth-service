using System.Linq.Expressions;
using Auth.Domain.Specification;

namespace Auth.Domain.Entities;

public interface IAsyncRepository<TEntity>
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity?> GetAsync(ISingleSpecification<TEntity> specification);
    Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity>? specification = null);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);
}
