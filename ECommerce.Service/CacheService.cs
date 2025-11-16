using ECommerce.Doamin.Contracts;
using ECommerce.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacherepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacherepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacherepository.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
        {
            var value=JsonSerializer.Serialize(cacheValue);
            await _cacherepository.SetAsync(cacheKey, value, timeToLive);
        }
    }
}
