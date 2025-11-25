using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class AuthenticationController:ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result=await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result=await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }

        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _authenticationService.CheckEmailAsync(email);
            return Ok(Result);
        }
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _authenticationService.GetUserByEmailAsync(email!);
            return HandleResult(user);
        }


        [HttpGet("GetAddress")]
        public async Task<ActionResult<UserDTO>> GetAddress()
        {

        }


    }
}
