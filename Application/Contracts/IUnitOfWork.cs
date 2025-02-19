using Domain.Entities;

namespace Application.Contracts;

public interface IUnitOfWork
{
    IGenericRepository<Book> Book { get; }
}
