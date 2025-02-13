using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Lookups;
using Repository.Context;
using Repository.GenericRepository;

namespace Implementation.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    //private readonly IEntityTracker EntityTracker;
    private readonly AppDbContext Context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public UnitOfWork(AppDbContext context)
    {
        //EntityTracker = entityTracker;
        Context = context;
    }

    private IGenericRepository<MainCategory>? mainCategory;
    public IGenericRepository<MainCategory> MainCategory => mainCategory ??= new GenericRepository<MainCategory>(Context);
}
