using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace S4_A_MVC.Controllers
{
    public class MenuController : Controller
    {
        [Route("Menu/General")]
        [Route("Menu/Index")]
        public IActionResult Index(string? lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                Debug.WriteLine($"Langue: {lang}");
            }
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [Route("Menu/Matin")]
        public IActionResult Dejeuner()
        {
            return View();
        }

        [Route("Menu/Midi")]
        public IActionResult Diner()
        {
            return View();
        }
    }
}