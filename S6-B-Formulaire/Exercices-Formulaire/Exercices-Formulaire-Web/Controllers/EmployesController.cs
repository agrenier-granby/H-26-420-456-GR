using Exercice_Formulaire_Web.Services;
using Exercices_Formulaire_Web.Models;
using Exercices_Formulaire_Web.Services;
using Exercices_Formulaire_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exercices_Formulaire_Web.Controllers
{
    public class EmployesController : Controller
    {
        private readonly EmployesService _employesService;
        private readonly DepartementsService _departementsService;
        private readonly PaysService _paysService;

        public EmployesController(
            EmployesService employesService,
            DepartementsService departementsService,
            PaysService paysService)
        {
            _employesService = employesService;
            _departementsService = departementsService;
            _paysService = paysService;
        }

        public async Task<IActionResult> Index()
        {
            var employes = await _employesService.GetAllAsync();
            var vm = employes.Select(e => new EmployeIndexVM
            {
                Id = e.Id,
                Nom = e.Nom,
                Age = e.Age,
                DateEmbauche = e.DateEmbauche,
                SalaireAnnuel = e.SalaireAnnuel,
                EstEnEmploi = e.EstEnEmploi,
                PaysOrigineNom = e.PaysOrigine?.Nom ?? "N/A",
                DepartementNom = e.Departement?.Nom ?? "N/A"
            }).ToList();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new EmployeCreateVM();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Nom = vm.Nom,
                    Age = vm.Age,
                    DateEmbauche = vm.DateEmbauche,
                    SalaireAnnuel = vm.SalaireAnnuel,
                    EstEnEmploi = vm.EstEnEmploi,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.AddAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employe = await _employesService.FindAsync(id);

            if (employe == null)
            {
                return NotFound();
            }

            var vm = new EmployeEditVM
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                EstEnEmploi = employe.EstEnEmploi,
                PaysId = employe.PaysId,
                DepartementId = employe.DepartementId,
                PaysOrigine = employe.PaysOrigine,
                Departement = employe.Departement
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeEditVM vm)
        {
            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Id = vm.Id,
                    Nom = vm.Nom,
                    Age = vm.Age,
                    DateEmbauche = vm.DateEmbauche,
                    SalaireAnnuel = vm.SalaireAnnuel,
                    EstEnEmploi = vm.EstEnEmploi,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.UpdateAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }
    }
}
