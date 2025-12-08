using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.SecurityDTOs
{
    public class AddressDTO()
    {
        public string City { get; init; }
        public string Country { get; init; }
        public string Street { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}
