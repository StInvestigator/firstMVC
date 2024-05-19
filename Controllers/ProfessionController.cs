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
            await _professionService.AddOrEditUser(id!=null? id.Value:_professionService.professions.Count == 0 ? 0 : _professionService.professions.Keys.Last() + 1, form);
            return RedirectToAction("ProfessionsList");
        }
        public async Task<IActionResult> DeleteProfession(int id)
        {
            foreach (var item in _userService.users.Where(pr => pr.Value.ProfessionId == id))
            {
                item.Value.ProfessionId = -1;
            }
            _professionService.professions.Remove(id);
            await _professionService.SaveAsync();
            return RedirectToAction("ProfessionsList");
        }
    }
}
