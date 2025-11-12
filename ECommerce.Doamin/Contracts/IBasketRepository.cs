using ECommerce.Doamin.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface IBasketRepository
    {
        Task<CostumerBasket?> GetBasketAsync(string basketId);
        Task<CostumerBasket> CreateOrUpdateBasketAsync(CostumerBasket basket,TimeSpan timeToLive=default);
        Task DeleteBasketAsync(string basketId);
    }
}
