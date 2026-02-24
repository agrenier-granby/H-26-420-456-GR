using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S5_B_LT_LINQ.Data;
using S5_B_LT_LINQ.Models;

namespace S5_B_LT_LINQ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult LinqExemple1()
        {
            var employes = _context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .ToList();
            return View("LinqExemple", employes);
        }

        public IActionResult LinqExemple2(int salaire = 50_000)
        {
            var employes = _context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .Where(e => e.SalaireAnnuel > salaire)
                .OrderBy(e => e.Age)
                .ToList();
            return View("LinqExemple", employes);
        }

        public IActionResult LinqExemple3()
        {
            var departement = _context.Departements
                .FirstOrDefault();

            var employes = _context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .Where(e => e.PaysOrigine.Nom == "Canada" && e.Departement == departement)
                .OrderByDescending(e => e.SalaireAnnuel)
                .ToList();
            return View("LinqExemple", employes);
        }
    }
}
