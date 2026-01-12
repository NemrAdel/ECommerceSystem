using ECommerce.Doamin.Contracts;
using ECommerce.Services.Abstraction;
using System.Text.Json;


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
            var value=JsonSerializer.Serialize(cacheValue,new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase,
            } );   // for camal case when return in cache memory
            await _cacherepository.SetAsync(cacheKey, value, timeToLive);
        }
    }
}
