using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Application.Contracts;
using Repository.Context;

namespace Repository.GenericRepository;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _entities;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        return await ApplyFilters(new QueryParameters<T> { Filter = filter, Include = include }).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(QueryParameters<T> parameters) => await ApplyFilters(parameters).ToListAsync();
    public async Task<int> GetTotalAsync(Expression<Func<T, bool>>? filter = null) => await ApplyFilters(new QueryParameters<T> { Filter = filter }).CountAsync();

    public void Insert(T entity) => _entities.Add(entity);
    public void Insert(IEnumerable<T> entities) => _entities.AddRange(entities);
    public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;
    public void Update(IEnumerable<T> entities) => _context.UpdateRange(entities);
    public void Delete(T entity) => _entities.Remove(entity);
    public void Delete(IEnumerable<T> entities) => _entities.RemoveRange(entities);

    public int ExecuteCommand(string query, params object[] parameters) => _context.Database.ExecuteSqlRaw(query, parameters);
    public int ExecuteStoredProcedure(string name, params object[] parameters) => _context.Database.ExecuteSqlRaw($"EXEC {name}", parameters);
    public int SaveChanges() => _context.SaveChanges();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

    private IQueryable<T> ApplyFilters(QueryParameters<T> parameters)
    {
        IQueryable<T> query = _entities.AsQueryable();

        if (parameters.Include != null)
            query = parameters.Include(query);

        if (parameters.Filter != null)
            query = query.Where(parameters.Filter);

        if (parameters.OrderBy != null)
            query = parameters.OrderBy(query);

        if (parameters.PageIndex.HasValue && parameters.PageSize.HasValue)
            query = query.Skip((parameters.PageIndex.Value - 1) * parameters.PageSize.Value).Take(parameters.PageSize.Value);

        return query.AsNoTracking();
    }
}
