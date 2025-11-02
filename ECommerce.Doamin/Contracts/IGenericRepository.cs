using ECommerce.Doamin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface IGenericRepository<Tentity,TKey>where Tentity : BaseEntity<TKey>
    {
        Task <IEnumerable<Tentity>> GetAllAsync();
        Task<Tentity?> GetByIdAsync(TKey id);
        Task AddAsync(Tentity entity);
        void Update(Tentity entity);
        void Delete(Tentity entity);

    }
}
