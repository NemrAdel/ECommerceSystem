using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.IdentityDTOs;
using ECommerce.Shared.DTOs.SecurityDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISecurityRepository<Address> _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager
            ,IConfiguration configuration,
            ISecurityRepository<Address> context,
            IMapper mapper,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user=await _userManager.FindByEmailAsync(loginDTO.Email);
            if(user is null)
            {
                return Error.InvalidCredintals("User.InvalidCredintials"); 
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
            {
                return Error.InvalidCredintals("User.InvalidCredintials");

            }
            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName!, token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
                
            };
            var IdentityResult = await _userManager.CreateAsync(user, registerDTO.Password);
            if (IdentityResult.Succeeded)
            {
                var token = await CreateTokenAsync(user);
                return new UserDTO(user.Email, user.DisplayName, token);
            }
            return IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),// instead of "Email" for confirm name
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = _configuration["JWTOptions:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);// use symetric key to encrypt and decrypt the key in the same server 
            var token =new JwtSecurityToken                                                                     //var token = new JwtSecurityToken
            (
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims:claims,
                signingCredentials:cred

            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With the email {email} is not found");
            return new UserDTO(user.Email!, user.DisplayName, await CreateTokenAsync(user));
        }

        public async Task<Result<AddressDTO>> GetAddress(string email)
        {
            var user = await _userManager.Users.
                Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            if(user is null)
                return Error.NotFound("User.NotFound", $"User With the email {email} is not found");
            var Address = await _context.GetAddressAsync(user.Id.ToString());

            var MappedAddress = _mapper.Map<AddressDTO>(Address);
            return MappedAddress;

        }

        public async Task<Result<AddressDTO>> UpdateAddress(string email, AddressDTO addressToUpdate)
        {
            var user = await _userManager.Users.
                Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With the email {email} is not found");
            if(user.Address is not null)
            {
                user.Address.City = addressToUpdate.City;
                user.Address.Country = addressToUpdate.Country;
                user.Address.Street = addressToUpdate.Street;
                user.Address.FirstName = addressToUpdate.FirstName;
                user.Address.LastName = addressToUpdate.LastName;    
                //user.Address.UpdateAt = DateTime.Now;
            }
            else // create new address
            {
                user.Address=_mapper.Map<Address>(addressToUpdate);
            }

            var result=await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDTO>(user.Address);
        }
        public async Task<Result<bool>> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Error.NotFound("User Not Found", $"User {user} Not Found");
            }
            var OTP = new Random().Next(100000, 999999).ToString();
            user.OTP = OTP;
            user.OTPExpireDate = DateTime.Now.AddMinutes(10);
            user.UpdatedAt = DateTime.Now;
            await _userManager.UpdateAsync(user);
            var emailDto = new EmailDTO
            {
                To = email,
                Subject = "Password Reset Request",
                Body = "Your OTP is :- \n" +
                $" -- {OTP} -- \n" +
                "This OTP is valid for 10 minutes. If you did not request a password reset, please ignore this email And Try Agian later.😍 \n\n" +
                "Don't Share This With AnyOne 🤫🤫" +
                "Best Regards,\n" +
                "Talabat Management Team ⚡"
            };
            await _emailService.SendEmailAsync(emailDto);

            return true ;
        }

        public async Task<Result<bool>> ResetPassword(string email, int otp, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                {

                    return Error.NotFound("User Not Found" , $"User {user} is not found");
                }
                if (user.OTP != otp.ToString())
                {
                    return Error.NotFound("Invalid OTP", "The provided OTP is invalid.");
                }
                if (user.OTPExpireDate < DateTime.Now)
                {
                    return Error.InvalidCredintals("OTP Is Expired" , "Your OTP Is Expired");
                }

                var passwordHasher = _userManager.PasswordHasher;
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, newPassword);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {

                    return Error.InvalidCredintals("Same Password","You Enter The same Password");
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (!result.Succeeded)
                {
                    return Error.InvalidCredintals("Password Reset Failed", "Failed to reset the password.");
                }
                var emailDto = new EmailDTO
                {
                    To = email,
                    Subject = "Password Reset Successful ⚡✅🔒",
                    Body = "Your password has been reset successfully. If you did not perform this action, please contact support immediately.🔒 \n\n" +
                    "Don't Forget It And Stay Secure 🔒 " +
                    "Best Regards,\n" +
                    "Talabat Management Team ⚡"
                };
                await _emailService.SendEmailAsync(emailDto);
                user.OTP = null;
                user.OTPExpireDate = null;
                user.UpdatedAt = DateTime.Now;
                await _userManager.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                return Error.Failure("Server Error", ex.Message);
            }
        }
    }
}
