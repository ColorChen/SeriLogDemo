using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeriLogDemo_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkForRedisCacheController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redisCache;

        public WorkForRedisCacheController(IConnectionMultiplexer redisCache) {
            _redisCache = redisCache;
        }

        // GET: api/<WorkForMemoryCacheController>
        [HttpGet]
        [Route("/GetFromAbsoluteRedisCache")]
        public string GetFromAbsoluteCache()
        {
            var result = "";
            var cache =  _redisCache.GetDatabase(0).StringGet(MemoryCacheKey.Work);
            if (!cache.HasValue)
            {
                DateTime now = DateTime.Now;

                var cacheValue = $"{now.ToString("yyyy/MM/dd HH:mm:ss")}";

                // every five second
                _redisCache.GetDatabase(0).StringSet(MemoryCacheKey.Work, cacheValue, TimeSpan.FromSeconds(5));
                result = cacheValue;
            }
            else {
                result = cache;
            }
            return result;
        }

        // GET: api/<WorkForMemoryCacheController>
        [HttpGet]
        [Route("/GetFromRedisCacheNeverExpire")]
        public string GetFromRacheCacheNeverExpire()
        {
            var result = "";
            var cache = _redisCache.GetDatabase(0).StringGet(MemoryCacheKey.WorkNotExpire);
            if (!cache.HasValue)
            {
                DateTime now = DateTime.Now;

                var cacheValue = $"{now.ToString("yyyy/MM/dd HH:mm:ss")}";

                _redisCache.GetDatabase(0).StringSet(MemoryCacheKey.WorkNotExpire, cacheValue);
                result = cacheValue;
            }
            else
            {
                result = cache;
            }
            return result;
        }


    }
}
