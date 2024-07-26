using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace firstMVC.Controllers
{
    public class ReviewController(SiteContext _context) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ReviewModalAjax(int id)
        {
            return PartialView(await _context.Users.FirstAsync(x => x.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(int id, [FromBody] ReviewForm form)
        {
            try
            {
                var user = await _context.Users.FirstAsync(x => x.Id == id);

                var review = new Review
                {
                    Name = form.Name,
                    Text = form.Text,
                    Rating = form.Rating,
                    CreatedAt = DateTime.Now,
                    Status = CurrentStatus.OnModeration
                };

                user?.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return Json(new { Ok = true });
            }
            catch
            {
                return Json(new { Ok = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsListAjax(int id)
        {
            return PartialView(await _context.Users
                .Include(x=>x.Reviews)
                .FirstAsync(x => x.Id == id));
        }
    }
}
