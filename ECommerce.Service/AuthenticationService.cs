using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.IdentityDTOs;
using ECommerce.Shared.DTOs.SecurityDTOs;
using Microsoft.AspNetCore.Identity;
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

        public AuthenticationService(
            UserManager<ApplicationUser> userManager
            ,IConfiguration configuration,
            ISecurityRepository<Address> context
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
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
            var user = await _userManager.FindByEmailAsync(email);
            var Address = await _context.GetAddressAsync(user.Id.ToString());
            var MappedAddress = new AddressDTO(City: Address.City, Country: Address.Country, Street: Address.Street, FirstName: Address.FirstName, LastName: Address.LastName, userId: Address.UserId);
            return MappedAddress;

        }

        public async Task<Result<bool>> UpdateAddress(string email, AddressDTO addressToUpdate)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var Address = await _context.GetAddressAsync(user.Id.ToString());


            Address.City = addressToUpdate.City;
            Address.Country = addressToUpdate.Country;
            Address.Street = addressToUpdate.Street;
            Address.FirstName = addressToUpdate.FirstName;
            Address.LastName = addressToUpdate.LastName;
            Address.UserId = addressToUpdate.userId;
            
            return await _context.UpdateAddressAsync(Address);
            



        }
    }
}
