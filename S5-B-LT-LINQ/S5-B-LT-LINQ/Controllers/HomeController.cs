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

        public IActionResult EagerLoading()
        {
            // EAGER LOADING : Chargement anticipé avec Include()
            // Génère UNE SEULE requête SQL avec des JOINs
            // Exemple SQL : 
            // SELECT TOP(100) e.*, p.*, d.*
            // FROM Employes e
            // LEFT JOIN Pays p ON e.PaysId = p.Id
            // LEFT JOIN Departements d ON e.DepartementId = d.Id

            var employes = _context.Employes
                .Include(e => e.PaysOrigine)      // JOIN avec Pays
                .Include(e => e.Departement)      // JOIN avec Departements
                .Take(100)
                .ToList();

            // Résultat: TOUTES les données sont chargées en mémoire en 1 seul aller-retour DB
            return View(employes);
        }

        public IActionResult LazyLoading()
        {
            // LAZY LOADING : Chargement paresseux avec proxies et virtual
            // Génère PLUSIEURS requêtes SQL (problème N+1)

            // 1ère requête : Récupère SEULEMENT les employés
            // SELECT TOP(100) * FROM Employes
            var employes = _context.Employes
                .Take(100)
                .ToList();

            // ATTENTION: Chaque accès à une propriété de navigation génère une NOUVELLE requête!
            //foreach (var e in employes)
            //{
            //    // À chaque itération, 1 requête pour charger le Departement
            //    // SELECT * FROM Departements WHERE Id = @p0
            //    System.Diagnostics.Debug.WriteLine(e.Departement?.Nom);
            //}

            // Résultat pour 100 employés: 
            // - 1 requête pour les employés
            // - 100 requêtes pour PaysOrigine (dans la vue)
            // - 100 requêtes pour Departement (dans la vue + foreach)
            // = 201 requêtes SQL au total! (Problème N+1)

            return View(employes);
        }
    }
}
