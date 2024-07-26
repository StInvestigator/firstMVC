using firstMVC.Areas.Moderator.Models.Forms;
using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = "Moderator")]
    public class UserController(SiteContext _context) : Controller
    {

        public async Task<IActionResult> UsersList()
        {
            return View(await _context.Users
                .Include(x => x.Image)
                .Include(x => x.Profession)
                .Where(x => x.CurrentStatus == CurrentStatus.OnModeration)
                .ToListAsync());
        }

        public async Task<IActionResult> OnUserReject(int id)
        {
            var model = await _context.Users.FirstAsync(s => s.Id == id);
            model.CurrentStatus = CurrentStatus.Rejected;

            await _context.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }

        public async Task<IActionResult> OnUserUpload(int id)
        {
            var model = await _context.Users.FirstAsync(s => s.Id == id);
            model.CurrentStatus = CurrentStatus.Uploaded;

            await _context.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }
        public async Task<IActionResult> UserGallery(int id)
        {
            return View((await _context.Users.Include(x => x.Gallery).ToListAsync()).Find(us => us.Id == id));
        }

        
    }
}
