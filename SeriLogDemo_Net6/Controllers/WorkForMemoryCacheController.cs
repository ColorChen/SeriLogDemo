using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeriLogDemo_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkForMemoryCacheController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public WorkForMemoryCacheController(IMemoryCache memoryCache) { 
            _memoryCache = memoryCache;
        }

        // GET: api/<WorkForMemoryCacheController>
        [HttpGet]
        [Route("/GetFromAbsoluteCache")]
        public IEnumerable<string> GetFromAbsoluteCache()
        {
            var result = new List<string>();
            if (!_memoryCache.TryGetValue(MemoryCacheKey.Work, out List<string> cacheValue))
            {
                DateTime now = DateTime.Now;
                
                cacheValue = new List<string> { $"{now.ToString("yyyy/MM/dd HH:mm:ss")}", $"{now.AddSeconds(1).ToString("yyyy/MM/dd HH:mm:ss")}" };

                // 方法一：比較[現在呼叫時間] - [上次呼叫時間]差異，時間內如果有被存取，則繼續展延
                //var cacheEntryOptions = new MemoryCacheEntryOptions()
                //    .SetSlidingExpiration(TimeSpan.FromSeconds(3));
                //_memoryCache.Set(MemoryCacheKey.Work, cacheValue, cacheEntryOptions);

                // 方法二：比較[現在呼叫時間] - [建立時間]差異，一超過就重新產生
                _memoryCache.Set(MemoryCacheKey.Work, cacheValue, TimeSpan.FromSeconds(3));
            }

            result = cacheValue;
            return result;
        }

        // GET: api/<WorkForMemoryCacheController>
        [HttpGet]
        [Route("/GetFromRalationCache")]
        public IEnumerable<string> GetFromRalationCache()
        {
            var result = new List<string>();
            if (!_memoryCache.TryGetValue(MemoryCacheKey.Work, out List<string> cacheValue))
            {
                DateTime now = DateTime.Now;

                cacheValue = new List<string> { $"{now.ToString("yyyy/MM/dd HH:mm:ss")}", $"{now.AddSeconds(1).ToString("yyyy/MM/dd HH:mm:ss")}" };

                //方法一：比較[現在呼叫時間] - [上次呼叫時間]差異，時間內如果有被存取，則繼續展延
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(3));
                _memoryCache.Set(MemoryCacheKey.Work, cacheValue, cacheEntryOptions);


            }

            result = cacheValue;
            return result;
        }

        //// GET api/<WorkForMemoryCacheController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<WorkForMemoryCacheController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<WorkForMemoryCacheController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<WorkForMemoryCacheController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
