using firstMVC.Areas.Moderator.Models.Forms;
using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace firstMVC.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    [Authorize(Roles = "Moderator")]
    public class ReviewController(SiteContext _context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ModerateAjax(int id)
        {
            return PartialView(await _context.Users
                .Include(x=>x.Reviews)
                .FirstAsync(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> RejectReview([FromBody] ModerateReview mod)
        {
            try
            {
                var model = await _context.Users.Include(x=>x.Reviews).FirstAsync(s => s.Id == mod.UserId);
                model.Reviews.First(x=>x.Id==mod.ReviewId).Status = CurrentStatus.Rejected;

                await _context.SaveChangesAsync();
                return Json(new { OK = true });
            }
            catch
            {
                return Json(new { OK = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadReview([FromBody] ModerateReview mod)
        {
            try
            {
                var model = await _context.Users.Include(x => x.Reviews).FirstAsync(s => s.Id == mod.UserId);
                model.Reviews.First(x => x.Id == mod.ReviewId).Status = CurrentStatus.Uploaded;

                await _context.SaveChangesAsync();
                return Json(new { OK = true });
            }
            catch
            {
                return Json(new { OK = false });
            }
        }
    }
}
