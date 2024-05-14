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
            _userService = new UserService("users.json");
        }
        public IActionResult UsersList()
        {
            return View(_userService.users);
        }
        [HttpGet]
        public IActionResult UserForm(int? id)
        {
            try
            {
                if (id != null)
                {
                    return View(_userService.users[id.Value]);
                }
            }
            catch { }
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> UserForm(int? id, [FromForm] User form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            await _userService.AddOrEditUser(id!=null?id.Value:_userService.users.Count == 0 ? 0: _userService.users.Keys.Last() + 1, form);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            _userService.users.Remove(id);
            await _userService.SaveAsync();
            return RedirectToAction("UsersList");
        }
    }
}
