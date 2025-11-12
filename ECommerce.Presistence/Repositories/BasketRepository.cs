using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.BasketModule;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public BasketRepository(IConnectionMultiplexer connection)
        {
            
        }
        public Task<CostumerBasket> CreateOrUpdateBasketAsync(CostumerBasket basket, TimeSpan timeToLive = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBasketAsync(string basketId)
        {
            throw new NotImplementedException();
        }

        public Task<CostumerBasket?> GetBasketAsync(string basketId)
        {
            throw new NotImplementedException();
        }
    }
}
