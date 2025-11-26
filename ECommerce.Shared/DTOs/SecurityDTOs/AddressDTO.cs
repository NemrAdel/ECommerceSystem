using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.SecurityDTOs
{
    public record AddressDTO(string City , string Country , string Street , string FirstName , string LastName, string userId)
    {
    }
}
