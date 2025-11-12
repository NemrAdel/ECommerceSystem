using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.BasketModule;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CostumerBasket> CreateOrUpdateBasketAsync(CostumerBasket basket, TimeSpan timeToLive = default)
        {
            var jsonBasket=JsonSerializer.Serialize(basket);
            var isCreatingOrUpdateing = await _database.StringSetAsync
                (basket.Id, jsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            return await GetBasketAsync(basket.Id);
            
        }

        public async Task DeleteBasketAsync(string basketId)=>
            await _database.KeyDeleteAsync(basketId);
        

        public async Task<CostumerBasket?> GetBasketAsync(string basketId)
        {
            var basket= await _database.StringGetAsync(basketId);
            if (basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CostumerBasket>(basket!);
        }
    }
}
