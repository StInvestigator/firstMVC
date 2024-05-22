using firstMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace firstMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutMe()
        {
            AboutMe aboutMe = new AboutMe("���������", "�������", 17, "��������� � ������������, ����� ��������� � ����", "/img/default/Gigachad.jpg");


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
