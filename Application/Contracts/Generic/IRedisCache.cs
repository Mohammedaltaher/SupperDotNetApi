
namespace Application.Contracts;

public interface IRedisCache
{
    Task<string> GetAsync(string? key);

    Task<string> UpdateAsync(string? key, string? data, int? absoluteExpiration = null, int? slidingExpiration = null);

    Task DeleteAsync(string? key);
}