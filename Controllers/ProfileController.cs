using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace firstMVC.Controllers
{
    [Authorize]
    public class ProfileController(UserManager<Customer> _userManager, SignInManager<Customer> _signInManager) : Controller
    {
        public async Task<Customer> GetCurrentUserAsync()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return await _userManager
                .Users
                .FirstAsync(x => x.Id.ToString() == userId);
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetCurrentUserAsync());
        }

        [HttpGet]
        public IActionResult ChangePasswordFormModalAjax()
        {
            return PartialView(new ChangePasswordForm());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordFormModalAjax([FromBody] ChangePasswordForm form)
        {
            var user = await GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(form);
            }
            if (!await _userManager.CheckPasswordAsync(user, form.Password))
            {
                ModelState.AddModelError(nameof(form.Password), "Невірний пароль");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(form);
            }
            if (form.NewPassword != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Паролі не співпадають");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(form);
            }

            var result = await _userManager.ChangePasswordAsync(user, form.Password, form.NewPassword);


            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Password), string.Join("; ", result.Errors.Select(e => e.Description)));
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(form);
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return Json(new { Ok = true });
        }

        [HttpGet]
        public async Task<IActionResult> ChangeDataFormModalAjax()
        {
            var user = await GetCurrentUserAsync();
            var currentInfo = new ChangeCustomerDataForm()
            {
                Name = user.FullName ?? "",
                Email = user.Email ?? ""
            };
            return PartialView(currentInfo);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDataFormModalAjax([FromBody] ChangeCustomerDataForm form)
        {
            var user = await GetCurrentUserAsync();
            var currentInfo = new ChangeCustomerDataForm()
            {
                Name = user.FullName ?? "",
                Email = user.Email ?? ""
            };
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(currentInfo);
            }
            if (await _userManager.FindByEmailAsync(form.Email)!=null && user.Email != form.Email)
            {
                ModelState.AddModelError(nameof(form.Email), "Користувач з поштою " + form.Email + " вже істнує");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(currentInfo);
            }

            user.Email = form.Email;
            user.FullName = form.Name;


            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Name), string.Join("; ", result.Errors.Select(e => e.Description)));
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView(form);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await SingIn(user);

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            return Json(new { Ok = true });
        }

        private async Task SingIn(Customer user)
        {
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? "nullEmail"));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName ?? "nullName"));
            var userRoles = await _userManager.GetRolesAsync(user);
            userRoles.ToList().ForEach(r =>
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, r));
            });

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
        }
    }
}
