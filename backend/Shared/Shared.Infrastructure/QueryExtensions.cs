using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace Shared.Infrastructure;

public static class QueryExtensions
{
    public static IQueryable<T> ApplyQuery<T>(
        this IQueryable<T> dbQuery, 
        Query query, 
        Dictionary<string, Expression<Func<T, bool>>> filterMappings,
        Dictionary<string, Expression<Func<T, object>>> sortMappings) 
        where T : class
    {
        if (!string.IsNullOrWhiteSpace(query.Filter) && filterMappings.TryGetValue(query.FilterBy.ToLower() ?? string.Empty, out var filterExpression))
        {
            dbQuery = dbQuery.Where(filterExpression);
        }
        
        if (sortMappings.TryGetValue(query.OrderBy?.ToLower() ?? string.Empty, out var sortExpression))
        {
            return query.OrderDesc ? dbQuery.OrderByDescending(sortExpression) : dbQuery.OrderBy(sortExpression);
        }

        return dbQuery;
    }

    public static async Task<PagedList<T2>> ApplyPaging<T1, T2>(this IQueryable<T1> source, Query query, Func<T1, T2> mapFunc, CancellationToken ct) 
        where T1: class
        where T2: class
    {
        var total = await source.CountAsync();
        var items = await source.Skip((query.PageNo - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(i => mapFunc(i))
            .ToListAsync(ct);

        return new PagedList<T2>(items, query.PageNo, query.PageSize, total);
    }
}