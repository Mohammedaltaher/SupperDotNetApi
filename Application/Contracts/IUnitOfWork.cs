using Domain.Entities.Lookups;

namespace Application.Contracts;

public interface IUnitOfWork
{
    IGenericRepository<MainCategory> MainCategory { get; }
}
