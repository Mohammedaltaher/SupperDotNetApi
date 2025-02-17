using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(QueryParameters<T> parameters);

    Task<int> GetTotalAsync(Expression<Func<T, bool>>? filter = null);

    void Insert(T entity);
    void Insert(IEnumerable<T> entity);
    void Update(T entity);
    void Update(IEnumerable<T> entity);
    void Delete(T entity);
    void Delete(IEnumerable<T> entity);
    int ExecuteCommand(string query, params object[] parameters);
    int ExecuteStoredProcedure(string name, params object[] parameters);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
