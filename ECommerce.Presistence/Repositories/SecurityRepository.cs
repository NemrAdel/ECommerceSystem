using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Presistence.IdentityData.DbContext;
using ECommerce.Shared.DTOs.SecurityDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Repositories
{
    public class SecurityRepository<TEntity>:ISecurityRepository<Address>
    {
        private readonly StoreIdentityDbContext _context;

        public SecurityRepository(StoreIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetAddressAsync(string userId)
        {
            return await _context.Set<Address>().FirstOrDefaultAsync(a=>a.UserId==userId);

        }

        public  async Task<bool> UpdateAddressAsync(Address address)
        {
            var isUpdated = _context.Set<Address>().Update(address);
            return await _context.SaveChangesAsync()>0;

        }
    }
}
