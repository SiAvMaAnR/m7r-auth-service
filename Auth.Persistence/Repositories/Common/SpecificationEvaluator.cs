using Auth.Domain.Entities;
using Auth.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence.Repositories.Common;

public static class SpecificationEvaluator
{
    public static async Task<IQueryable<TEntity>> GetQueryForManyAsync<TEntity>(
        this IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification
    )
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> query = inputQuery;

        query = specification
            .Includes
            .Aggregate(query, (current, include) => current.Include(include));

        query = specification
            .IncludeStrings
            .Aggregate(query, (current, include) => current.Include(include));

        query = query.AsSplitQuery();

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }

        if (specification.IsAsNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await Task.FromResult(query);
    }

    public static Task<TEntity?> GetQueryForOneAsync<TEntity>(
        this IQueryable<TEntity> inputQuery,
        ISingleSpecification<TEntity> specification
    )
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> query = inputQuery;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification
            .Includes
            .Aggregate(query, (current, include) => current.Include(include));

        query = specification
            .IncludeStrings
            .Aggregate(query, (current, include) => current.Include(include));

        query = query.AsSplitQuery();

        if (specification.IsAsNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query.FirstOrDefaultAsync();
    }
}
