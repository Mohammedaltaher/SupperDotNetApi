//using Core.Helper.Implementations;
//using Domain.ViewModel;
//using MediatR;

//namespace Application.Utilities;

//[AttributeUsage(AttributeTargets.Class, Inherited = false)]
//public class CacheAttribute : Attribute
//{
//    public int DurationInMinutes { get; }
//    public CacheAttribute(int durationInMinutes) => DurationInMinutes = durationInMinutes;
//}

//public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : IRequest<TResponse>
//{
//    private readonly IRedisCache redisCache;
//    private bool IsRedisWorking = false;
//    public CacheBehavior(IRedisCache redisCache)
//    {
//        this.redisCache = redisCache;
//    }
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var cacheAttribute = request.GetType().GetCustomAttributes(typeof(CacheAttribute), false).FirstOrDefault() as CacheAttribute;
//        IsRedisWorking = await redisCache.IsRedisWorkingAsync();

//        if (cacheAttribute == null || !IsRedisWorking)
//            return await next();

//        var cacheKey = GenerateCacheKey(request);

//        var contentCache = await redisCache.GetAsync(cacheKey);

//        if (!string.IsNullOrEmpty(contentCache))
//            return JsonSerializer.Deserialize<TResponse>(contentCache)!;

//        var response = await next();

//        await redisCache.UpdateAsync(cacheKey, JsonSerializer.Serialize(response), 1, cacheAttribute.DurationInMinutes);

//        return response;
//    }
//    private static string GenerateCacheKey(TRequest request) => $"{typeof(TRequest).FullName}_{JsonSerializer.Serialize(request)}";
//}
