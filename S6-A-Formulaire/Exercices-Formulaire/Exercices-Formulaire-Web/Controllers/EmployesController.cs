using Exercice_Formulaire_Web.Services;
using Exercices_Formulaire_Web.Data;
using Exercices_Formulaire_Web.Services;
using Exercices_Formulaire_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exercices_Formulaire_Web.Controllers
{
    public class EmployesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployesService _employesService;
        private readonly DepartementsService _departementsService;
        private readonly PaysService _paysService;

        public EmployesController(
            ApplicationDbContext context,
            EmployesService employesService,
            DepartementsService departementsService,
            PaysService paysService)
        {
            _context = context;
            _employesService = employesService;
            _departementsService = departementsService;
            _paysService = paysService;
        }

        public async Task<IActionResult> Index()
        {
            var employes = await _employesService.GetAllAsync();
            var employesVM = employes.Select(e => new EmployeIndexVM
            {
                Id = e.Id,
                Nom = e.Nom,
                Age = e.Age,
                DateEmbauche = e.DateEmbauche,
                SalaireAnnuel = e.SalaireAnnuel,
                EstEnEmploi = e.EstEnEmploi,
                PaysOrigineNom = e.PaysOrigine?.Nom,
                DepartementNom = e.Departement?.Nom
            }).ToList();
            return View(employesVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new EmployeCreateVM { Nom = "Inconnu", Age = 18 };
            return View(vm);
        }
    };
}
