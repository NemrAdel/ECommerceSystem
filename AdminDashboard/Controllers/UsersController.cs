using AdminDashboard.Models.Roles;
using AdminDashboard.Models.Users;
using AdminDashboard.Views.Users;
using ECommerce.Doamin.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            var users = _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var userModel=new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new UpdateRoleViewModel
                {
                    UserId = role.Id,
                    Name = role.Name!,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name!).Result
                }).ToList()
            };
        }
    }
}
