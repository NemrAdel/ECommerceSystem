using AdminDashboard.Models.Roles;
using AdminDashboard.Models.Users;
using ECommerce.Doamin.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users =await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                UserName = user.UserName!,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var userModel = new UserRoleViewModel
            {
                UserId = user!.Id,
                UserName = user.UserName!,
                Roles = roles.Select(r => new UpdateRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name!,
                    IsSelected= _userManager.IsInRoleAsync(user, r.Name!).Result
                }).ToList()
            };
            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesForUser = await _userManager.GetRolesAsync(user!);
            foreach(var role in model.Roles)
            {
                if (rolesForUser.Any(r => r == role.Name) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user!, role.Name);
                }
                if (!rolesForUser.Any(r => r == role.Name) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user!, role.Name);
                }

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
