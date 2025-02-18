using System.Text.Json;

namespace Application.Utilities;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CacheAttribute : Attribute
{
    public int DurationInMinutes { get; }
    public CacheAttribute(int durationInMinutes) => DurationInMinutes = durationInMinutes;
}
public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRedisCache _redisCache;

    public CacheBehavior(IRedisCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheAttribute = request.GetType().GetCustomAttributes(typeof(CacheAttribute), false).FirstOrDefault() as CacheAttribute;
        if (cacheAttribute == null)
            return await next();

        var cacheKey = GenerateCacheKey(request);

        var cachedResponse = await _redisCache.GetAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            return JsonSerializer.Deserialize<TResponse>(cachedResponse)!;
        }

        var response = await next();
        await _redisCache.UpdateAsync(cacheKey, JsonSerializer.Serialize(response), 1, cacheAttribute.DurationInMinutes);
        return response;
    }

    private static string GenerateCacheKey(TRequest request)
    {
        return $"{typeof(TRequest).FullName}_{JsonSerializer.Serialize(request)}";
    }
}
