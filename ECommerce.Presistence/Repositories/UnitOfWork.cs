using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
using ECommerce.Presistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];
        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<Tentity, Tkey> GetRepository<Tentity, Tkey>() where Tentity : BaseEntity<Tkey>
        {
            var entityType = typeof(Tentity);
            if(_repositories.TryGetValue(entityType,out var repository))
            {
                return (IGenericRepository<Tentity,Tkey>)repository;
            }
            var newRepository = new GenericRepository<Tentity, Tkey>(_dbContext);
            _repositories[entityType]= newRepository;
            return newRepository;
        }

        public async Task<int> saveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
