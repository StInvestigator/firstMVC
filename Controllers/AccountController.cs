using firstMVC.Models;
using firstMVC.Models.Forms;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace firstMVC.Controllers
{
    public class AccountController(UserManager<Customer> _userManager) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterForm());
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterForm form, string? returnUrl)
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

            await SingIn(user);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginForm());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginForm form, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var user = await _userManager.FindByEmailAsync(form.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(form.Email), "Користувача з поштою " + form.Email + " не істнує");
                return View(form);
            }
            if (!await _userManager.CheckPasswordAsync(user, form.Password))
            {
                ModelState.AddModelError(nameof(form.Password), "Невірний пароль");
                return View(form);
            }

            await SingIn(user);

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task SingIn(Customer user)
        {
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email??"nullEmail"));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName??"nullName"));
            var userRoles = await _userManager.GetRolesAsync(user);
            userRoles.ToList().ForEach(r =>
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, r));
            });


            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
