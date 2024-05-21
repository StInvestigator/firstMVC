using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class SkillController : Controller
    {
        private readonly SkillService _skillService;
        private readonly UserService _userService;
        public SkillController(SkillService skillService, UserService userService)
        {
            _skillService = skillService;
            _userService = userService;
        }
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
                    return View(_skillService.skills[_skillService.skills.FindIndex(us => us.Id == id)]);
                }
            }
            catch { }
            return View(new Skill());
        }

        [HttpPost]
        public async Task<IActionResult> SkillForm(int? id, [FromForm] Skill form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            form.Id = id != null ? id.Value : _skillService.skills.Count == 0 ? 0 : _skillService.skills.Last().Id + 1;
            if (id != null)
            {
                _userService.users.ForEach(us =>
                {
                    var skill = us.Skills.Find(s => s.Skill.Id == id);
                    if (skill != null)
                    {
                        skill.Skill = form;
                    }
                });
            }
            await _skillService.AddOrEdit(form);
            return RedirectToAction("SkillsList");
        }
        public async Task<IActionResult> DeleteSkill(int id)
        {
            _userService.users.ForEach(us =>
            {
                us.Skills.Remove(us.Skills.Find(s => s.Skill.Id == id));
            });
            _skillService.skills.Remove(_skillService.skills.Find(us => us.Id == id));
            await _skillService.SaveAsync();
            return RedirectToAction("SkillsList");
        }
    }
}
