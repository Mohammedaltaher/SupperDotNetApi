using Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Core.Helper.Implementations
{
    public class RedisCacheService : IRedisCache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAsync(string? key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("Attempted to get a cache value with an empty key.");
                return string.Empty;
            }

            try
            {
                var cachedData = await _distributedCache.GetStringAsync(key);
                return string.IsNullOrEmpty(cachedData) ? string.Empty : cachedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve cache key: {Key}", key);
                return string.Empty;
            }
        }

        public async Task<string> UpdateAsync(string? key, string? data, int? absoluteExpiration = null, int? slidingExpiration = null)
        {
            if (string.IsNullOrEmpty(key) || data == null)
            {
                _logger.LogWarning("Invalid cache update request. Key or Data is null.");
                return string.Empty;
            }

            try
            {
                var cacheOptions = CreateCacheEntryOptions(absoluteExpiration, slidingExpiration);
                await _distributedCache.SetStringAsync(key, data, cacheOptions);
                return await GetAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update cache key: {Key}", key);
                return string.Empty;
            }
        }

        public async Task DeleteAsync(string? key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("Attempted to delete a cache value with an empty key.");
                return;
            }

            try
            {
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete cache key: {Key}", key);
            }
        }

        public async Task<bool> IsRedisWorkingAsync()
        {
            try
            {
                var testKey = "RedisHealthCheck";
                await _distributedCache.SetStringAsync(testKey, "OK", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5) });
                var result = await _distributedCache.GetStringAsync(testKey);
                return result == "OK";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis health check failed.");
                return false;
            }
        }

        private static DistributedCacheEntryOptions CreateCacheEntryOptions(int? absoluteExpiration = null, int? slidingExpiration = null)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration.HasValue ? TimeSpan.FromHours(absoluteExpiration.Value) : TimeSpan.FromHours(24),
                SlidingExpiration = slidingExpiration.HasValue ? TimeSpan.FromMinutes(slidingExpiration.Value) : TimeSpan.FromMinutes(60)
            };
        }
    }
}
