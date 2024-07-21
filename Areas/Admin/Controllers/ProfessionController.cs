using firstMVC.Areas.Admin.Models.Forms;
using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfessionController(SiteContext _context, LocalFileService _fileService) : Controller
    {
        public async Task<IActionResult> ProfessionsList()
        {
            return View(await _context.Professions.Include(x => x.Image).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> ProfessionForm(int? id)
        {
            try
            {
                if (id != null)
                {
                    return View(new ProfessionForm((await _context.Professions.Include(x => x.Image).ToListAsync()).Find(pr => pr.Id == id)));
                }
            }
            catch { }
            return View(new ProfessionForm());
        }

        [HttpPost]
        public async Task<IActionResult> ProfessionForm(int? id, [FromForm] ProfessionForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            Profession newProfession = new Profession
            {
                Name = form.Name,
                Image = form.Image == null ? null : await _fileService.CreateImage(form.Image)
            };
            if (id != null)
            {
                var pro = (await _context.Professions.Include(x => x.Image).ToListAsync()).Find(pr => pr.Id == id);
                if (pro?.Image != null)
                {
                    _fileService.DeleteImage(pro.Image);
                    _context.Remove(pro.Image);
                }
            }
            int i = _context.Professions.ToList().FindIndex(pr => pr.Id == id);

            if (i != -1)
            {
                var model = await _context.Professions.Include(x => x.Image).ElementAtAsync(i);
                model.Name = newProfession.Name;
                model.Image = newProfession.Image;
            }
            else _context.Professions.Add(newProfession);

            await _context.SaveChangesAsync();
            return RedirectToAction("ProfessionsList");
        }
        public async Task<IActionResult> DeleteProfession([FromBody] DeleteEntityByIdForm form)
        {
            try
            {
                var pro = (await _context.Professions.Include(x => x.Image).ToListAsync()).Find(pr => pr.Id == form.Id);
                if (pro?.Image != null)
                {
                    _fileService.DeleteImage(pro.Image);
                    _context.Remove(pro.Image);
                }
                foreach (var item in _context.Users.Include(x => x.Profession).Where(x => x.Profession == pro))
                {
                    item.Profession = null;
                }
                _context.Professions.Remove(pro);
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