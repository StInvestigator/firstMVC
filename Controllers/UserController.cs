using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class UserController (UserService _userService, ProfessionService _professionService, LocalFileService _fileService) : Controller
    {

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
                    return View(new UserForm(_userService.users[_userService.users.FindIndex(us=>us.Id==id)]));
                }
            }
            catch { }
            return View(new UserForm());
        }

        [HttpPost]
        public async Task<IActionResult> UserForm(int? id, [FromForm] UserForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["professions"] = _professionService.professions;
                return View(form);
            }
            Image? img = null;
            if (form.Image != null)
            {
                img = await _fileService.CreateImage(form.Image);
            }
            if (id != null)
            {
                var user = _userService.users.Find(us => us.Id == id);
                if (user?.Image != null)
                {
                    _fileService.DeleteImage(user.Image);
                }
                form.Id = id.Value;
            }
            else form.Id = _userService.users.Count == 0 ? 0 : _userService.users.Last().Id + 1;

            await _userService.AddOrEdit(form, img);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.users.Find(us => us.Id == id);
            if (user?.Image != null)
            {
                _fileService.DeleteImage(user.Image);
            }
            _userService.users.Remove(user);
            await _userService.SaveAsync();
            return RedirectToAction("UsersList");
        }
    }
}
