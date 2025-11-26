using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Contracts
{
    public interface ISecurityRepository<Address>
    {
        public Task<Address> GetAddressAsync(string userId);
        public Task<bool> UpdateAddressAsync(Address address);
    }
}
