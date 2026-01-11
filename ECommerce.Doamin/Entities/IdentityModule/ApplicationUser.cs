using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.IdentityModule
{
    public class ApplicationUser:IdentityUser
    {
        public string DisplayName { get; set; } = default!;
        public Address? Address { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpireDate { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
