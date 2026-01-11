using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await  _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) 
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginDTO);
            }

            var result =await  _signInManager.PasswordSignInAsync(user, loginDTO.Password, false,false);
            if (!result.Succeeded || (!await _userManager.IsInRoleAsync(user,"Admin") && (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))))
            {
                ModelState.AddModelError("", "You are not Authorized.");
                return View(loginDTO);
            }

            return RedirectToAction(nameof(Index),"Home");
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
