using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.IdentityDTOs;
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

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
    }
}
