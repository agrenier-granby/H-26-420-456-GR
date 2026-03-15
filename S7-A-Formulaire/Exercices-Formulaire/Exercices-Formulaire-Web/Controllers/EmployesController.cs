using Exercice_Formulaire_Web.Services;
using Exercices_Formulaire_Web.Models;
using Exercices_Formulaire_Web.Services;
using Exercices_Formulaire_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var employesVM = employes.Select(e => new EmployeIndexVM
            {
                Id = e.Id,
                Nom = e.Nom,
                Age = e.Age,
                DateEmbauche = e.DateEmbauche,
                SalaireAnnuel = e.SalaireAnnuel,
                Statut = e.Statut,
                TypeRemuneration = e.TypeRemuneration,
                PaysOrigineNom = e.PaysOrigine?.Nom,
                DepartementNom = e.Departement?.Nom
            }).ToList();

            return View(employesVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();

            var vm = new EmployeCreateVM
            {
                ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList(),
                ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList()
            };
            
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
                    Statut = vm.Statut,
                    TypeRemuneration = vm.TypeRemuneration,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.AddAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            // Recharger les listes en cas d'erreur
            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();
            vm.ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList();
            vm.ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList();

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

            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();

            var vm = new EmployeEditVM
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                Statut = employe.Statut,
                TypeRemuneration = employe.TypeRemuneration,
                PaysId = employe.PaysId,
                DepartementId = employe.DepartementId,
                PaysOrigine = employe.PaysOrigine,
                Departement = employe.Departement,
                ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList(),
                ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeEditVM vm)
        {

            if (vm.Nom == vm.PaysOrigine?.Nom)
            {
                ModelState.AddModelError(nameof(vm.Nom), "Le nom de l'employé ne peut pas être le même que le pays");
            }

            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Id = vm.Id,
                    Nom = vm.Nom,
                    Age = vm.Age,
                    DateEmbauche = vm.DateEmbauche,
                    SalaireAnnuel = vm.SalaireAnnuel,
                    Statut = vm.Statut,
                    TypeRemuneration = vm.TypeRemuneration,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.UpdateAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            // Recharger les listes en cas d'erreur
            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();
            vm.ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList();
            vm.ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList();

            return View(vm);
        }
    }
}
