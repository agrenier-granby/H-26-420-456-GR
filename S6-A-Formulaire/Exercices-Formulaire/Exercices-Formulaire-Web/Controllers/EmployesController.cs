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
            return View(employes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new EmployeCreateVM { Nom = "Inconnu", Age = 18 };
            return View(vm);
        }
    };
}
