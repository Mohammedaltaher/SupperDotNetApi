using Domain.Entities.Lookups;

namespace Application.Contracts;

public interface IUnitOfWork
{
    IGenericRepository<Book> Book { get; }
}
