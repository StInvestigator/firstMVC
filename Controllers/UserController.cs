using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class UserController (UserService _userService, ProfessionService _professionService) : Controller
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
            if(form.Image != null)
            {
                using (var fileStream = new FileStream("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + "\\img\\user\\" + form.Image.FileName, FileMode.Create))
                {
                    await form.Image.CopyToAsync(fileStream);
                }
            }
            if (id != null)
            {
                var user = _userService.users.Find(us => us.Id == id);
                if (user?.Image != null && _userService.users.Where(us => us.Image?.Path == user.Image.Path).Count()==1)
                {
                    System.IO.File.Delete("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + user.Image.Path);
                }
                form.Id = id.Value;
            }
            else form.Id = _userService.users.Count == 0 ? 0 : _userService.users.Last().Id + 1;

            await _userService.AddOrEdit(form);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.users.Find(us => us.Id == id);
            if (user?.Image != null && _userService.users.Where(us => us.Image.Path == user.Image.Path).Count() == 1)
            {
                System.IO.File.Delete("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + user.Image.Path);
            }
            _userService.users.Remove(user);
            await _userService.SaveAsync();
            return RedirectToAction("UsersList");
        }
    }
}
