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
            AboutMe aboutMe = new AboutMe("Владислав", "Мурашко", 17, "народився в Краматорську, зараз перебуваю в Одесі", "/img/Gigachad.jpg");
            List<Skill> skills = new List<Skill>() { 
                new Skill(0,"C#",70), 
                new Skill(1, "HTML + CSS", 80), 
                new Skill(2, "JavaScript", 80), 
                new Skill(3, "Python", 20) 
            };

            ViewData["AboutMe"] = aboutMe;
            ViewData["Skills"] = skills;
            return View();
        }
        
        [HttpGet]
        public IActionResult UserForm()
        {
            
            User user = new User();
            if (System.IO.File.Exists("D:\\SHAG\\visual save\\firstMVC\\wwwroot\\data\\UserData.txt"))
            {
                using (StreamReader reader = new StreamReader("D:\\SHAG\\visual save\\firstMVC\\wwwroot\\data\\UserData.txt"))
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        user.Name = line.Split(" ")[0];
                        user.Password = line.Split(" ")[1];
                    }
                    reader.Close();
                }
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult UserForm([FromForm] User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            StreamWriter writer = new StreamWriter("D:\\SHAG\\visual save\\firstMVC\\wwwroot\\data\\UserData.txt");
            writer.Write(user.Name + " " + user.Password);
            writer.Close();
            return Redirect("/Home/UserPage");
        }

        public IActionResult UserPage()
        {
            if (System.IO.File.Exists("D:\\SHAG\\visual save\\firstMVC\\wwwroot\\data\\UserData.txt"))
            {
                using (StreamReader reader = new StreamReader("D:\\SHAG\\visual save\\firstMVC\\wwwroot\\data\\UserData.txt"))
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        ViewData["UserName"] = line.Split(" ")[0];
                    }
                    reader.Close();
                }
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
