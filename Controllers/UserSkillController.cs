using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace firstMVC.Controllers
{
    [Authorize]
    public class UserSkillController(SiteContext _context) : Controller
    {
        public async Task<IActionResult> UserSkillsList()
        {
            return View(await _context.Users.Include(x => x.Skills).ThenInclude(x => x.Skill.Image).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> UserSkillForm(int userId, int? userSkillId)
        {
            ViewData["skills"] = await _context.Skills.ToListAsync();
            var skills = (await _context.Users.Include(x => x.Skills).FirstAsync(us => us.Id == userId))?.Skills;
            ViewData["userSkills"] = skills;
            ViewData["id"] = userSkillId;
            try
            {
                if (userSkillId != null)
                {
                    return View(new UserSkillForm { Mastery = skills.Find(sk => sk.Skill.Id == userSkillId).Mastery, SkillId = userSkillId.Value });
                }
            }
            catch { }
            return View(new UserSkillForm());
        }

        [HttpPost]
        public async Task<IActionResult> UserSkillForm(int userId, int? userSkillId, [FromForm] UserSkillForm form)
        {
            if (form.SkillId == -1)
            {
                return RedirectToAction("UserSkillsList");
            }
            var skills = (await _context.Users.Include(x => x.Skills).ThenInclude(x => x.Skill).FirstAsync(us => us.Id == userId))?.Skills;
            if (userSkillId != null)
            {
                skills[skills.FindIndex(sk => sk.Skill.Id == userSkillId)] = new UserSkill
                {
                    Skill = (await _context.Skills.ToListAsync()).Find(sk => sk.Id == form.SkillId),
                    Mastery = form.Mastery
                };
            }
            else
            {
                skills.Add(new UserSkill
                {
                    Skill = (await _context.Skills.ToListAsync()).Find(sk => sk.Id == form.SkillId),
                    Mastery = form.Mastery
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UserSkillsList");
        }
        public async Task<IActionResult> DeleteUserSkill([FromBody] DeleteEntityByDoubleIdForm form)
        {
            try
            {
                var skills = (await _context.Users.Include(x => x.Skills).ThenInclude(x => x.Skill).FirstAsync(us => us.Id == form.firstId))?.Skills;
                skills.Remove(skills.Find(sk => sk.Skill.Id == form.secondId));
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
