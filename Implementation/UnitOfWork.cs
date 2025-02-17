using Application.Contracts;
using Domain.Entities.Lookups;
using Repository.Context;
using Repository.GenericRepository;

namespace Implementation.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext Context;

    public UnitOfWork(AppDbContext context)
    {
        Context = context;
    }

    private IGenericRepository<MainCategory>? mainCategory;
    public IGenericRepository<MainCategory> MainCategory => mainCategory ??= new GenericRepository<MainCategory>(Context);
}
