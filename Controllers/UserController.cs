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
            List<Image>? gal = null;
            if (form.Image != null)
            {
                img = await _fileService.CreateImage(form.Image);
            }
            if(form.Gallery.Count != 0)
            {
                gal = new List<Image>();
                foreach (var item in form.Gallery)
                {
                    gal.Add(await _fileService.CreateImage(item));
                }
            }
            if (id != null)
            {
                var user = _userService.users.Find(us => us.Id == id);
                if (user?.Image != null)
                {
                    _fileService.DeleteImage(user.Image);
                }
                if (user?.Gallery != null)
                {
                    foreach (var item in user?.Gallery)
                    {
                        _fileService.DeleteImage(item);
                    }
                }
                form.Id = id.Value;
            }
            else form.Id = _userService.users.Count == 0 ? 0 : _userService.users.Last().Id + 1;
            await _userService.AddOrEdit(form, img, gal);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.users.Find(us => us.Id == id);
            if (user?.Image != null)
            {
                _fileService.DeleteImage(user.Image);
            }
            if(user?.Gallery != null)
            {
                foreach (var item in user?.Gallery)
                {
                    _fileService.DeleteImage(item);
                }
            }
            _userService.users.Remove(user);
            await _userService.SaveAsync();
            return RedirectToAction("UsersList");
        }
        public IActionResult UserGallery(int id)
        {
            return View(_userService.users.Find(us=>us.Id==id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteImageForm form)
        {
            try
            {
                var user = _userService.users.First(us => us.Id == form.UserId);
                var image = user?.Gallery?[form.ImageId];

                _fileService.DeleteImage(image);
                user.Gallery.Remove(image); 
                await _userService.SaveAsync();
                return Json(new {OK = true});

            }catch(Exception ex)
            {
                return Json(new { OK = false });
            }
        }
    }
}
