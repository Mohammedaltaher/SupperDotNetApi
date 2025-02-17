using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Contracts;

public class QueryParameters<T>
{
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public Expression<Func<T, bool>>? Filter { get; set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }
    public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; }
}
