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

    private IGenericRepository<Book>? book;
    public IGenericRepository<Book> Book => book ??= new GenericRepository<Book>(Context);
}
