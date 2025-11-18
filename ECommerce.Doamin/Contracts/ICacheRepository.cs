using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string casheKey);
        Task SetAsync(string casheKey, string cacheValue, TimeSpan timeToLive);
    }
}
