using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Controllers
{
    [Authorize]
    public class RolesController(UserManager<Customer> _userManager, RoleManager<IdentityRole<int>> _roleManager) : Controller
    {
        [HttpGet] 
        public async Task<IActionResult> CustomerRolesForm(int id)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewData["UserName"] = user.FullName;

            return View(new CustomerRolesForm
            {
                Roles = roles.Select(r => new CustomerRole
                {
                    Name = r.Name,
                    IsActive = userRoles.Contains(r.Name)
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CustomerRolesForm(int id, [FromForm] CustomerRolesForm form)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = user.FullName;

            var userRoles = await _userManager.GetRolesAsync(user);

            form.Roles.ForEach(async r =>
            {
                if (r.IsActive)
                {
                    await _userManager.AddToRoleAsync(user, r.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, r.Name);
                }
            });
            await _userManager.UpdateAsync(user);

            return RedirectToAction("CustomersList", "Customer");
        }

    }
}
