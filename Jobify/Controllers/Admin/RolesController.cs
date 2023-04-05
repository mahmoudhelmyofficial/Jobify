using Jobify.Data;
using Jobify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Controllers.Admin
{
        [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await context.Roles.ToListAsync();
            var rolesVm = new RolesViewModel { Roles = roles };
            return View(rolesVm);
        }
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            context.SaveChanges();

            return RedirectToAction("Index", "Roles");
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role is not null)
                await roleManager.DeleteAsync(role);
            else
                return NotFound();

            return RedirectToAction("Index", "Roles");
        }

    }
}
