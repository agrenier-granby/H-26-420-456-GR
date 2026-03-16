using Exercices_Formulaire_Web.Models;
using Exercices_Formulaire_Web.Services;
using Exercices_Formulaire_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exercices_Formulaire_Web.Controllers
{
    public class DepartementsController : Controller
    {
        private readonly DepartementsService _departementsService;

        public DepartementsController(DepartementsService departementsService)
        {
            _departementsService = departementsService;
        }

        public async Task<IActionResult> Index()
        {
            var departements = await _departementsService.GetAllAsync();
            var departementsVM = departements.Select(d => new DepartementIndexVM
            {
                Id = d.Id,
                Nom = d.Nom,
                Budget = d.Budget
            }).ToList();

            return View(departementsVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new FormulaireDepartementVM();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FormulaireDepartementVM vm)
        {
            if (ModelState.IsValid)
            {
                var departement = new Departement
                {
                    Nom = vm.Nom,
                    Budget = vm.Budget
                };

                await _departementsService.AddAsync(departement);
                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departement = await _departementsService.FindAsync(id);

            if (departement == null)
            {
                return NotFound();
            }

            var vm = new FormulaireDepartementVM
            {
                Id = departement.Id,
                Nom = departement.Nom,
                Budget = departement.Budget
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FormulaireDepartementVM vm)
        {
            if (ModelState.IsValid)
            {
                var departement = new Departement
                {
                    Id = vm.Id,
                    Nom = vm.Nom,
                    Budget = vm.Budget
                };

                await _departementsService.UpdateAsync(departement);
                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = await _departementsService.FindAsync(id.Value);

            if (departement == null)
            {
                return NotFound();
            }

            var vm = new FormulaireDepartementVM
            {
                Id = departement.Id,
                Nom = departement.Nom,
                Budget = departement.Budget
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var departement = await _departementsService.FindAsync(id);

            if (departement != null)
            {
                await _departementsService.RemoveAsync(departement);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
