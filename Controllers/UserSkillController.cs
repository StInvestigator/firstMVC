using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace firstMVC.Controllers
{
    public class UserSkillController : Controller
    {
        private readonly UserService _userService;
        private readonly SkillService _skillService;
        public UserSkillController(UserService userService, SkillService skillService)
        {
            _userService = userService;
            _skillService = skillService;
        }
        public IActionResult UserSkillsList()
        {
            return View(_userService.users);
        }
        [HttpGet]
        public IActionResult UserSkillForm(int userId,int? userSkillId)
        {
            ViewData["skills"] = _skillService.skills;
            var skills = _userService.users.Find(us => us.Id == userId)?.Skills;
            ViewData["userSkills"] = skills;
            ViewData["id"] = userSkillId;
            try
            {
                if (userSkillId != null)
                {
                    return View(new UserSkillForm { Mastery = skills[skills.FindIndex(sk => sk.Skill.Id == userSkillId)].Mastery, SkillId = userSkillId.Value });
                }
            }
            catch { }
            return View(new UserSkillForm());
        }

        [HttpPost]
        public async Task<IActionResult> UserSkillForm(int userId, int? userSkillId, [FromForm] UserSkillForm form)
        {
            if (form.SkillId==-1)
            {
                return RedirectToAction("UserSkillsList");
            }
            var skills = _userService.users.Find(us =>  us.Id == userId)?.Skills;
            if (userSkillId != null)
            {
                skills[skills.FindIndex(sk => sk.Skill.Id == userSkillId)] = new UserSkill
                {
                    Skill = _skillService.skills.Find(sk => sk.Id == form.SkillId),
                    Mastery = form.Mastery
                };
            }
            else
            {
                skills.Add(new UserSkill
                {
                    Skill = _skillService.skills.Find(sk=>sk.Id == form.SkillId),
                    Mastery = form.Mastery
                });
            }
            await _userService.SaveAsync();
            return RedirectToAction("UserSkillsList");
        }
        public async Task<IActionResult> DeleteUserSkill(int userId, int userSkillId)
        {
            var skills = _userService.users.Find(us => us.Id == userId)?.Skills;
            skills.Remove(skills.Find(sk => sk.Skill.Id == userSkillId));
            await _userService.SaveAsync();
            return RedirectToAction("UserSkillsList");
        }
    }
}
