﻿using firstMVC.Areas.Admin.Models.Forms;
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
    public class SkillController(SiteContext _context, LocalFileService _fileService) : Controller
    {
        public async Task<IActionResult> SkillsList()
        {
            return View(await _context.Skills.Include(x => x.Image).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> SkillForm(int? id)
        {
            try
            {
                if (id != null)
                {
                    return View(new SkillForm((await _context.Skills.Include(x => x.Image).ToListAsync()).Find(sk => sk.Id == id)));
                }
            }
            catch { }
            return View(new SkillForm());
        }

        [HttpPost]
        public async Task<IActionResult> SkillForm(int? id, [FromForm] SkillForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var newSkill = new Skill
            {
                Name = form.Name,
                Image = form.Image == null ? null : await _fileService.CreateImage(form.Image)
            };
            if (id != null)
            {
                var skill = (await _context.Skills.Include(x => x.Image).ToListAsync()).Find(us => us.Id == id);
                if (skill?.Image != null)
                {
                    _fileService.DeleteImage(skill.Image);
                    _context.Remove(skill.Image);
                }
            }
            int i = (await _context.Skills.ToListAsync()).FindIndex(pr => pr.Id == id);

            if (i != -1)
            {
                var model = await _context.Skills.Include(x => x.Image).ElementAtAsync(i);
                model.Name = newSkill.Name;
                model.Image = newSkill.Image;
            }
            else _context.Skills.Add(newSkill);

            await _context.SaveChangesAsync();
            return RedirectToAction("SkillsList");
        }
        public async Task<IActionResult> DeleteSkill([FromBody] DeleteEntityByIdForm form)
        {
            try
            {
                var skill = (await _context.Skills.Include(x => x.Image).ToListAsync()).Find(us => us.Id == form.Id);
                if (skill?.Image != null)
                {
                    _fileService.DeleteImage(skill.Image);
                    _context.Remove(skill.Image);
                }
                foreach (var us in _context.Users)
                {
                    var userSkill = us.Skills?.Find(s => s.Skill.Id == skill?.Id);
                    if (userSkill != null)
                    {
                        us.Skills?.Remove(userSkill);
                    }
                }
                _context.Skills.Remove(skill);
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
