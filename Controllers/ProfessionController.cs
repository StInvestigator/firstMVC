using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly UserService _userService;
        private readonly ProfessionService _professionService;
        public ProfessionController(UserService userService, ProfessionService professionService)
        {
            _userService = userService;
            _professionService = professionService;
        }
        public IActionResult ProfessionsList()
        {
            return View(_professionService.professions);
        }
        [HttpGet]
        public IActionResult ProfessionForm(int? id)
        {
            try
            {
                if (id != null)
                {
                    return View(_professionService.professions[id.Value]);
                }
            }
            catch { }
            return View(new Profession());
        }

        [HttpPost]
        public async Task<IActionResult> ProfessionForm(int? id, [FromForm] Profession form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            form.Id = id != null ? id.Value : _professionService.professions.Count == 0 ? 0 : _professionService.professions.Last().Id + 1;
            await _professionService.AddOrEdit(form);
            return RedirectToAction("ProfessionsList");
        }
        public async Task<IActionResult> DeleteProfession(int id)
        {
            foreach (var item in _userService.users.Where(us => us.ProfessionId == id))
            {
                item.ProfessionId = -1;
            }
            _professionService.professions.Remove(_professionService.professions.Find(pr=>pr.Id == id));
            await _professionService.SaveAsync();
            return RedirectToAction("ProfessionsList");
        }
    }
}
