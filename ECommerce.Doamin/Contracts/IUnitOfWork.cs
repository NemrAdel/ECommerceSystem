using ECommerce.Doamin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> saveChangesAsync();

        IGenericRepository<Tentity,Tkey> GetRepository<Tentity,Tkey>()where Tentity:BaseEntity<Tkey>;
    }
}
