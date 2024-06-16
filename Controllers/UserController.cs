using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Controllers
{
    public class UserController (SiteContext _context, LocalFileService _fileService) : Controller
    {

        public async Task<IActionResult> UsersList()
        {
            ViewData["professions"] = await _context.Professions.ToListAsync();
            return View(await _context.Users
                .Include(x=>x.Image)
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
                    return View(new UserForm((await _context.Users.Include(x => x.Image).ToListAsync()).Find(x=>x.Id==id)));
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
            Image? img = form.Image==null ? null: await _fileService.CreateImage(form.Image);
            List<Image>? gal = null;
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
                form.Id = id.Value;
                var user = (await _context.Users.Include(x=>x.Image).ToListAsync()).Find(us => us.Id == id);
                if (user?.Image != null)
                {
                    _fileService.DeleteImage(user.Image);
                    _context.Remove(user.Image);
                }
                if (user?.Gallery != null)
                {
                    foreach (var item in user?.Gallery)
                    {
                        _fileService.DeleteImage(item);
                        _context.Remove(item);
                    }
                }
            }

            await _context.AddOrEditUser(form, img, gal);
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = (await _context.Users
                .Include(x => x.Image)
                .Include(x => x.Gallery)
                .Include(x=>x.Skills).ThenInclude(x=>x.Skill)
                .ToListAsync()).Find(us => us.Id == id);
            if (user?.Image != null)
            {
                _fileService.DeleteImage(user.Image);
                _context.Remove(user.Image);
            }
            if(user?.Gallery != null)
            {
                foreach (var item in user?.Gallery)
                {
                    _fileService.DeleteImage(item);
                    _context.Remove(item);
                }
            }
            if(user?.Skills.Count > 0)
            {
                foreach (var item in user?.Skills)
                {
                    _context.UserSkills.Remove(item);   
                }
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> UserGallery(int id)
        {
            return View((await _context.Users.Include(x=>x.Gallery).ToListAsync()).Find(us=>us.Id==id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteImageForm form)
        {
            try
            {
                var user = await _context.Users.Include(x => x.Image).Include(x => x.Gallery).FirstAsync(us => us.Id == form.UserId);
                var image = user?.Gallery?[form.ImageId];

                _fileService.DeleteImage(image);
                user?.Gallery?.Remove(image); 

                _context.Remove(image);

                await _context.SaveChangesAsync();
                return Json(new {OK = true});

            }catch(Exception ex)
            {
                return Json(new { OK = false });
            }
        }
    }
}
