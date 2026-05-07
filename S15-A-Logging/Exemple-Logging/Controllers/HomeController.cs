using Exemple_Logging.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exemple_Logging.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        public IActionResult Index()
        {
            logger.LogDebug($"Requête HTTP dans [{nameof(Index)}]");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logger.LogCritical($"Requête HTTP dans [{nameof(Index)}]");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
