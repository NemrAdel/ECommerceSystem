using AdminDashboard.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Name", "Role Already Exists");
            }
            return View(nameof(Index),await _roleManager.Roles.ToListAsync());
        }

        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                ModelState.AddModelError("Id", "Role Not Found with this id");
                return RedirectToAction(nameof(Index));
            }
            UpdateRoleViewModel updateRoleViewModel = new UpdateRoleViewModel
            {
                Id = role.Id,
                Name = role.Name!
            };
            return View(updateRoleViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Already Exists");
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
