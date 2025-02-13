//using Core.Helper.Contracts;

//namespace Domain.ViewModel;


//public static class RedisCacheExtensions
//{

//    public static async Task<bool> IsRedisWorkingAsync(this IRedisCache redisCache)
//    {
//        const string testKey = "RedisTestKey";
//        const string testValue = "RedisTestValue";

//        try
//        {
//            // Set a value in the Redis cache
//            await redisCache.UpdateAsync(testKey, testValue);

//            // Get the value from the Redis cache
//            var cachedValue = await redisCache.GetAsync(testKey);
//            await redisCache.DeleteAsync(testKey);
            
//            // Verify the value
//            return cachedValue == testValue;
//        }
//        catch
//        {
//            // If any exception occurs, return false
//            return false;
//        }
//    }


//}