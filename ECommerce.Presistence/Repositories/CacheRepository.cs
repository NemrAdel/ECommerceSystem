using ECommerce.Doamin.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;

        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string casheKey)
        {
            var cacheValue = await _database.StringGetAsync(casheKey);
            return cacheValue.IsNullOrEmpty ? null : cacheValue.ToString();
        }

        public async Task SetAsync(string casheKey, string cacheValue, TimeSpan timeToLive)
        {
            await _database.StringSetAsync(casheKey, cacheValue, timeToLive);
        }
    }
}
