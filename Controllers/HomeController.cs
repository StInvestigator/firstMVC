using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace firstMVC.Controllers
{
    public class HomeController(SiteContext _context) : Controller
    {
        public async Task<IActionResult> Index([FromForm] HomeSearchForm form)
        {
            ViewData["SearchForm"] = form;

            if(form.Text == null)
            {
                form.Text = string.Empty;
            }

            var data = await _context.Users
                .Where(x=> x.Name.Contains(form.Text) 
                || (x.Profession==null? "Безробітний".Contains(form.Text) : x.Profession.Name.Contains(form.Text)))
                .Include(x=>x.Profession)
                .Include(x=>x.Image)
                .ToListAsync();
            return View(data);
        }
        public IActionResult AboutMe()
        {
            AboutMe aboutMe = new AboutMe("Владислав", "Мурашко", 17, "народився в Краматорську, зараз перебуваю в Одесі", "/uploads/img/default/Gigachad.jpg");

            ViewData["AboutMe"] = aboutMe;

            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
