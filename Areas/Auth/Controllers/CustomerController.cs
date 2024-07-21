using firstMVC.Areas.Auth.Models.Forms;
using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Roles = "Admin")]
    public class CustomerController(UserManager<Customer> _userManager) : Controller
    {
        public async Task<IActionResult> CustomersList()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer([FromBody] DeleteEntityByIdForm form)
        {
            try
            {
                var model = await _userManager.FindByIdAsync(form.Id.ToString());
                if (model == null)
                {
                    throw new Exception("Not found");
                }

                var result = await _userManager.DeleteAsync(model);

                return Json(new { OK = result.Succeeded });
            }
            catch
            {
                return Json(new { OK = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordForm(string id)
        {
            ViewData["User"] = await _userManager.FindByIdAsync(id);
            return View(new ResetPasswordForm());
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordForm(string id, [FromForm] ResetPasswordForm form)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewData["User"] = user;

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            if (form.Password != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Паролі повинні співпадати");
                return View(form);
            }

            var result = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), form.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Password), string.Join("; ", result.Errors.Select(e => e.Description)));
                return View(form);
            }
            return RedirectToAction("CustomersList", "Customer");
        }

        [HttpGet]
        public IActionResult CustomerForm()
        {
            return View(new RegisterForm());
        }

        [HttpPost]
        public async Task<IActionResult> CustomerForm([FromForm] RegisterForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            if (form.Password != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Паролі повинні співпадати");
                return View(form);
            }
            if (await _userManager.FindByEmailAsync(form.Email) != null)
            {
                ModelState.AddModelError(nameof(form.Email), "Користувач з поштою " + form.Email + " вже істнує");
                return View(form);
            }

            var user = new Customer
            {
                Email = form.Email,
                UserName = form.Email,
                FullName = form.Name
            };

            var result = await _userManager.CreateAsync(user, form.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Name), string.Join("; ", result.Errors.Select(e => e.Description)));
                return View(form);
            }

            return RedirectToAction("CustomersList", "Customer");
        }
    }
}
