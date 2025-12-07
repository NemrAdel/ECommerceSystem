using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.IdentityDTOs;
using ECommerce.Shared.DTOs.SecurityDTOs;
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

        public AuthenticationService(
            UserManager<ApplicationUser> userManager
            ,IConfiguration configuration,
            ISecurityRepository<Address> context,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
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
            if(Address is null)
                return Error.NotFound("Address.NotFound", $"Address for User With the email {email} is not found");

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
    }
}
