using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController(SiteContext _context, LocalFileService _fileService, UserManager<Customer> _userManager) : Controller
    {

        public async Task<IActionResult> UsersList()
        {
            var currentUser = await this.GetCurrentUserAsync(_userManager);

            return View(await _context.Users
                .Include(x => x.Image)
                .Include(x => x.Creator)
                .ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> UserForm(int? id)
        {
            ViewData["professions"] = await _context.Professions.ToListAsync();
            try
            {
                if (id != null)
                {
                    return View(new UserForm((await _context.Users.Include(x => x.Image).ToListAsync()).Find(x => x.Id == id)));
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
                ViewData["professions"] = await _context.Professions.ToListAsync();
                return View(form);
            }
            Image? img = form.Image == null ? null : await _fileService.CreateImage(form.Image);
            List<Image> gal = new List<Image>();
            if (form.Gallery.Count != 0)
            {
                foreach (var item in form.Gallery)
                {
                    gal.Add(await _fileService.CreateImage(item));
                }
            }
            if (id != null)
            {
                form.Id = id.Value;
                var user = (await _context.Users.Include(x => x.Image).ToListAsync()).Find(us => us.Id == id);
                if (user?.Image != null)
                {
                    _fileService.DeleteImage(user.Image);
                    _context.Remove(user.Image);
                }

                foreach (var item in user.Gallery)
                {
                    _fileService.DeleteImage(item);
                    _context.Remove(item);
                }

            }

            await _context.AddOrEditUser(form, img, gal, await this.GetCurrentUserAsync(_userManager));
            return RedirectToAction("UsersList");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteEntityByIdForm form)
        {
            try
            {
                var user = (await _context.Users
                    .Include(x => x.Image)
                    .Include(x => x.Gallery)
                    .Include(x => x.Reviews)
                    .Include(x => x.Skills).ThenInclude(x => x.Skill)
                    .ToListAsync()).Find(us => us.Id == form.Id);
                if (user?.Image != null)
                {
                    _fileService.DeleteImage(user.Image);
                    _context.Remove(user.Image);
                }
                foreach (var item in user.Gallery)
                {
                    _fileService.DeleteImage(item);
                    _context.Remove(item);
                }
                foreach (var item in user.Reviews)
                {
                    _context.Remove(item);
                }

                foreach (var item in user.Skills)
                {
                    _context.UserSkills.Remove(item);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Json(new { OK = true });

            }
            catch (Exception ex)
            {
                return Json(new { OK = false });
            }
        }
        public async Task<IActionResult> UserGallery(int id)
        {
            return View((await _context.Users.Include(x => x.Gallery).ToListAsync()).Find(us => us.Id == id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteEntityByDoubleIdForm form)
        {
            try
            {
                var user = await _context.Users.Include(x => x.Image).Include(x => x.Gallery).FirstAsync(us => us.Id == form.firstId);
                var image = user?.Gallery?[form.secondId];

                _fileService.DeleteImage(image);
                user?.Gallery?.Remove(image);

                _context.Remove(image);

                await _context.SaveChangesAsync();
                return Json(new { OK = true });
            }
            catch (Exception ex)
            {
                return Json(new { OK = false });
            }
        }
    }
}
