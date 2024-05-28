using firstMVC.Models;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace firstMVC.Controllers
{
    public class ProfessionController (UserService _userService, ProfessionService _professionService, LocalFileService _fileService) : Controller
    {
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
                    return View(new ProfessionForm(_professionService.professions[id.Value]));
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
                Id = id != null ? id.Value : _professionService.professions.Count == 0 ? 0 : _professionService.professions.Last().Id + 1,
                Name = form.Name,
                Image = form.Image==null ? null : await _fileService.CreateImage(form.Image)
            };
            if(id!=null)
            {
                var pro = _professionService.professions.Find(pr => pr.Id == id);
                if (pro?.Image != null)
                {
                    _fileService.DeleteImage(pro.Image);
                }
            }
            await _professionService.AddOrEdit(newProfession);
            return RedirectToAction("ProfessionsList");
        }
        public async Task<IActionResult> DeleteProfession(int id)
        {
            foreach (var item in _userService.users.Where(us => us.ProfessionId == id))
            {
                item.ProfessionId = -1;
            }
            var pro = _professionService.professions.Find(pr => pr.Id == id);
            if(pro?.Image != null)
            {
                _fileService.DeleteImage(pro.Image);
            }
            _professionService.professions.Remove(pro);
            await _professionService.SaveAsync();
            return RedirectToAction("ProfessionsList");
        }
    }
}