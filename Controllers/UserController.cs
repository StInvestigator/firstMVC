using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ProfessionService _professionService;
        public UserController(UserService userService, ProfessionService professionService)
        {
            _userService = userService;
            _professionService = professionService;
        }
        public IActionResult UsersList()
        {
            ViewData["professions"] = _professionService.professions;
            return View(_userService.users);
        }
        [HttpGet]
        public IActionResult UserForm(int? id)
        {
            ViewData["professions"] = _professionService.professions;
            try
            {
                if (id != null)
                {
                    return View(_userService.users[_userService.users.FindIndex(us=>us.Id==id)]);
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
                ViewData["professions"] = _professionService.professions;
                return View(form);
            }
            form.Id = id != null ? id.Value : _userService.users.Count == 0 ? 0 : _userService.users.Last().Id + 1;
            await _userService.AddOrEdit(form);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            _userService.users.Remove(_userService.users.Find(us=>us.Id==id));
            await _userService.SaveAsync();
            return RedirectToAction("UsersList");
        }
    }
}
