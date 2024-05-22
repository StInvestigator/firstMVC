using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class SkillController (SkillService _skillService, UserService _userService) : Controller
    {
        public IActionResult SkillsList()
        {
            return View(_skillService.skills);
        }
        [HttpGet]
        public IActionResult SkillForm(int? id)
        {
            try
            {
                if (id != null)
                {
                    return View(new SkillForm(_skillService.skills[_skillService.skills.FindIndex(us => us.Id == id)]));
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
            var newSkill = new Skill { 
                Id = id != null ? id.Value : _skillService.skills.Count == 0 ? 0 : _skillService.skills.Last().Id + 1, 
                Name = form.Name, 
                Image = form.Image == null ? null : new Image { Name = form.Image.FileName, Path = "\\img\\skill\\" + form.Image.FileName }
            };
            if (id != null)
            {
                _userService.UpdateSkills(newSkill);
                var skill = _skillService.skills.Find(us => us.Id == id);
                if (skill?.Image != null && _skillService.skills.Where(us => us.Image?.Path == skill.Image.Path).Count() == 1)
                {
                    System.IO.File.Delete("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + skill.Image.Path);
                }
            }
            if (form.Image != null)
            {
                using (var fileStream = new FileStream("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + "\\img\\skill\\" + form.Image.FileName, FileMode.Create))
                {
                    await form.Image.CopyToAsync(fileStream);
                }
            }
            await _skillService.AddOrEdit(newSkill);
            return RedirectToAction("SkillsList");
        }
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = _skillService.skills.Find(us => us.Id == id);
            if (skill?.Image != null && _userService.users.Where(us => us.Image?.Path == skill.Image.Path).Count() == 1)
            {
                System.IO.File.Delete("D:\\SHAG\\visual save\\firstMVC\\wwwroot" + skill.Image.Path);
            }
            _userService.DeleteSkills(skill);
            _skillService.skills.Remove(skill);
            await _skillService.SaveAsync();
            return RedirectToAction("SkillsList");
        }
    }
}
