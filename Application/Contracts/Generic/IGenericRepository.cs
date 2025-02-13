using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

    Task<int> GetTotalAsync(Expression<Func<T, bool>>? filter = null);

    Task<T?> GetAsyncWithIgnoreQueryFilters(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
    Task<IEnumerable<T>> GetAllWithIgnoreQueryFilterAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllWithIgnoreQueryFilterAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Insert(T entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Insert(IEnumerable<T> entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Update(IEnumerable<T> entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Delete(T entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Delete(IEnumerable<T> entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>

    int ExecuteCommand(string query, params object[] parameters);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    int ExecuteStoredProcedure(string name, params object[] parameters);
    /// <summary>
    /// Save Changes in data base
    /// </summary>
    /// <returns></returns>
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
