using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Repository.Context;
using Microsoft.Extensions.Configuration;
using Application.Contracts;

namespace Repository.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    //private readonly IEntityTracker EntityTracker;
    private readonly AppDbContext Context;
    private readonly DbSet<T> Entitie;
    private IQueryable<T> Query;

    //private IConfiguration configuration
    //{
    //    get
    //    {
    //        return (IConfiguration)HttpContextAccessorUtility.Current()!.RequestServices.GetService(typeof(IConfiguration))!;
    //    }
    //}

    public GenericRepository(AppDbContext context)
    {
        //EntityTracker = entityTracker;
        Context = context;
        Entitie = context.Set<T>();
        Query = Entitie;
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        return await QueryCreator(filter, null, includes, null).IgnoreQueryFilters().AsSingleQuery().FirstOrDefaultAsync();
    }

    #region Get All

    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        return await QueryCreator(filter, orderBy, includes, null).AsSingleQuery().ToListAsync();
    }
    public virtual async Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        return await QueryCreator(filter, orderBy, includes, null).AsSingleQuery().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<T?> GetAsyncWithIgnoreQueryFilters(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? includes = null)
    {
        return await QueryCreator(filter, null, includes, null).IgnoreQueryFilters().AsSingleQuery().FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<T>> GetAllWithIgnoreQueryFilterAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? includes = null)
    {
        return await QueryCreator(filter, orderBy, includes, null).IgnoreQueryFilters().AsSingleQuery().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<IEnumerable<T>> GetAllWithIgnoreQueryFilterAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? includes = null)
    {
        return await QueryCreator(filter, orderBy, includes, null).IgnoreQueryFilters().AsSingleQuery().ToListAsync();
    }
    public virtual async Task<int> GetTotalAsync(Expression<Func<T, bool>>? filter = null)
    {
        var list = await QueryCreator(filter, null, null, null).IgnoreQueryFilters().AsSingleQuery().ToListAsync();
        return list.Count;
    }

    #endregion

    #region Command
    public virtual void Insert(T entity)
    {
        Entitie.Add(entity);
        Task.Run(() => DeleteKeys(typeof(T).Name));
    }

    public virtual void Insert(IEnumerable<T> entity)
    {
        Entitie.AddRange(entity);
        Task.Run(() => DeleteKeys(typeof(T).Name));
    }

    public virtual void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        Task.Run(() => DeleteKeys(typeof(T).Name));

    }

    public virtual void Update(IEnumerable<T> entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        Task.Run(() => DeleteKeys(typeof(T).Name));

    }

    public virtual void Delete(T entity)
    {
        Entitie.Remove(entity);
        Task.Run(() => DeleteKeys(typeof(T).Name));

    }

    public virtual void Delete(IEnumerable<T> entity)
    {
        Entitie.RemoveRange(entity);
        Task.Run(() => DeleteKeys(typeof(T).Name));

    }

    public int ExecuteCommand(string query, params object[] parameters)
    {
        return Context.Database.ExecuteSqlRaw(query, parameters);
    }

    public int ExecuteStoredProcedure(string name, params object[] parameters)
    {
        return Context.Database.ExecuteSqlRaw($"EXEC {name}", parameters);
    }

    public int SaveChanges()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = true;
        return Context.SaveChanges();
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        List<string> tables = new List<string>()
        {
        };

        Context.ChangeTracker.AutoDetectChangesEnabled = true;
        var saved = Context.SaveChangesAsync(cancellationToken);
        //var UpdatedData = EntityTracker.Savechanges(Context, tables);
        return saved;
    }
    #endregion Command

    #region Helper Funs
    private IQueryable<T> QueryCreator(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, params Expression<Func<T, object>>[]? includes)
    {
        if (include is not null)
        {
            Query = include(Query);
        }
        else
        {
            if (includes is not null)
                foreach (var item in includes)
                    Query = Query.Include(item);
        }

        if (filter != null)
            Query = Query.Where(filter);

        if (orderBy != null)
            Query = orderBy(Query);

        return Query.AsNoTracking();
    }

    private async Task DeleteKeys(string pattern)
    {
        try
        {
            //RedisCacheViewModel redisCache = configuration.GetSettings<RedisCacheViewModel>("CacheSettings");

            //var redis = ConnectionMultiplexer.Connect($"{redisCache.ConnectionString},password={redisCache.Password}");
            //var db = redis.GetDatabase();
            //var server = redis.GetServer($"{redisCache.ConnectionString}");

            //var keys = server.Keys(pattern: $"*{pattern}*").ToList(); // Convert to list for processing

            //var deleteTasks = keys.Select(key => db.KeyDeleteAsync(key)); // Create a list of tasks for deletion

            //await Task.WhenAll(deleteTasks); // Wait for all tasks to complete

            //redis.Dispose();
        }
        catch
        {
            //ignore
        }

    }
    #endregion
}
