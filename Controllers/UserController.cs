using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController()
        {
            _userService = new UserService("user.json");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserForm()
        {
            return View(_userService.user);
        }

        [HttpPost]
        public async Task<IActionResult> UserForm([FromForm] User form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var user = _userService.user;
            user.Name = form.Name;
            user.Password = form.Password;
            user.Age = form.Age;
            user.Balance = form.Balance;
            user.Birthday = form.Birthday;
            user.IsMale = form.IsMale;
            await _userService.SaveAsync();
            return RedirectToAction("UserPage");
        }

        public IActionResult UserPage()
        {
            return View(_userService.user);
        }
    }
}
