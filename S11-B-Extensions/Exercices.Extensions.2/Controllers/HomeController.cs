using Exercices.Extensions.Models;
using Exercices.Extensions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exercices.Extensions.Controllers
{
    public class HomeController(DiagnosticsService diag) : Controller
    {
        private readonly DiagnosticsService _diagnostics = diag;

        public IActionResult Index()
        {
            return View(_diagnostics.GetDiagnostics());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}