using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.IdentityDTOs;
using ECommerce.Shared.DTOs.SecurityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
        Task<Result<AddressDTO>> GetAddress(string email);
        Task<Result<AddressDTO>> UpdateAddress(string email,AddressDTO address);
        Task<Result<bool>> ForgetPassword(string email);
        Task<Result<bool>> ResetPassword(string email, int otp, string password);

    }
}
