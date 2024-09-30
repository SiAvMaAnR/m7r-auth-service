using System.Linq.Expressions;
using Auth.Domain.Entities;
using Auth.Domain.Specification;
using Auth.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence.Repositories.Common;

public class BaseRepository<TEntity> : IAsyncRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(EFContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<TEntity?> GetAsync(ISingleSpecification<TEntity> specification)
    {
        return await _dbSet.GetQueryForOneAsync(specification);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity>? specification
    )
    {
        return await _dbSet.GetQueryForManyAsync(specification ?? new DefaultSpec<TEntity>());
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public virtual async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AllAsync(predicate);
    }
}
