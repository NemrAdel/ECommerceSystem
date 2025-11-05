using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
using ECommerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{
    public class GenericRepository<Tentity, Tkey> : IGenericRepository<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Tentity entity)
        {
          await _dbContext.Set<Tentity>().AddAsync(entity);
        }

        public void Delete(Tentity entity)
        {
            _dbContext.Set<Tentity>().Remove(entity);
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync(Expression<Func<Tentity,bool>>? condition=default)
        {
            if (condition is not null)
            {
                return await _dbContext.Set<Tentity>().Where(condition).ToListAsync();
            }
            return await _dbContext.Set<Tentity>().ToListAsync();
        }

        public async Task<Tentity?> GetByIdAsync(Tkey id)
        {
            return await _dbContext.Set<Tentity>().FindAsync(id);
        }

        public void Update(Tentity entity)
        {
            _dbContext.Set<Tentity>().Update(entity);
        }
    }
}
