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
                DateNaissance = e.DateNaissance,
                DateEmbauche = e.DateEmbauche,
                SalaireAnnuel = e.SalaireAnnuel,
                Titre = e.Titre,
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
            var vm = new EmployeCreateVM
            {
                DateNaissance = DateTime.Today.AddYears(-18),
                DateEmbauche = DateTime.Today
            };

            await RemplirListesAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeCreateVM vm)
        {
            if (vm.Titre == Titre.GestionnaireDeProjet)
            {
                var dejaUnGestionnaire = await _employesService.DepartementADejaGestionnaireAsync(vm.DepartementId);
                if (dejaUnGestionnaire)
                {
                    ModelState.AddModelError(nameof(vm.DepartementId), "Cette équipe a déjà un gestionnaire de projet.");
                }
            }

            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Nom = vm.Nom!.Trim(),
                    Age = vm.Age ?? 18,
                    DateNaissance = vm.DateNaissance!.Value,
                    DateEmbauche = vm.DateEmbauche!.Value,
                    DateDepart = vm.DateDepart,
                    SalaireAnnuel = vm.SalaireAnnuel ?? 40000,
                    Titre = vm.Titre!.Value,
                    Statut = vm.Statut,
                    TypeRemuneration = vm.TypeRemuneration,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.AddAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            AjouterErreursAuSommaire();
            await RemplirListesAsync(vm);
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
                DateNaissance = employe.DateNaissance,
                DateEmbauche = employe.DateEmbauche,
                DateDepart = employe.DateDepart,
                SalaireAnnuel = employe.SalaireAnnuel,
                Titre = employe.Titre,
                Statut = employe.Statut,
                TypeRemuneration = employe.TypeRemuneration,
                PaysId = employe.PaysId,
                DepartementId = employe.DepartementId,
                PaysOrigine = employe.PaysOrigine,
                Departement = employe.Departement
            };

            await RemplirListesAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeEditVM vm)
        {
            if (vm.Titre == Titre.GestionnaireDeProjet)
            {
                var dejaUnGestionnaire = await _employesService.DepartementADejaGestionnaireAsync(vm.DepartementId, vm.Id);
                if (dejaUnGestionnaire)
                {
                    ModelState.AddModelError(nameof(vm.DepartementId), "Cette équipe a déjà un gestionnaire de projet.");
                }
            }

            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Id = vm.Id,
                    Nom = vm.Nom!.Trim(),
                    Age = vm.Age ?? 18,
                    DateNaissance = vm.DateNaissance!.Value,
                    DateEmbauche = vm.DateEmbauche!.Value,
                    DateDepart = vm.DateDepart,
                    SalaireAnnuel = vm.SalaireAnnuel ?? 40000,
                    Titre = vm.Titre!.Value,
                    Statut = vm.Statut,
                    TypeRemuneration = vm.TypeRemuneration,
                    PaysId = vm.PaysId,
                    DepartementId = vm.DepartementId
                };

                await _employesService.UpdateAsync(employe);
                return RedirectToAction(nameof(Index));
            }

            AjouterErreursAuSommaire();
            await RemplirListesAsync(vm);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employe = await _employesService.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }

            var vm = new EmployeIndexVM
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateNaissance = employe.DateNaissance,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                Titre = employe.Titre,
                Statut = employe.Statut,
                TypeRemuneration = employe.TypeRemuneration,
                PaysOrigineNom = employe.PaysOrigine?.Nom,
                DepartementNom = employe.Departement?.Nom
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employe = await _employesService.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }

            var vm = new EmployeIndexVM
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateNaissance = employe.DateNaissance,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                Titre = employe.Titre,
                Statut = employe.Statut,
                TypeRemuneration = employe.TypeRemuneration,
                PaysOrigineNom = employe.PaysOrigine?.Nom,
                DepartementNom = employe.Departement?.Nom
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employe = await _employesService.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }

            await _employesService.RemoveAsync(employe);
            return RedirectToAction(nameof(Index));
        }

        private async Task RemplirListesAsync(EmployeCreateVM vm)
        {
            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();
            vm.ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList();
            vm.ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList();
        }

        private async Task RemplirListesAsync(EmployeEditVM vm)
        {
            var pays = await _paysService.GetAll();
            var departements = await _departementsService.GetAllAsync();
            vm.ListePays = pays.Select(p => new SelectListItem(p.Nom, p.Id.ToString())).ToList();
            vm.ListeDepartements = departements.Select(d => new SelectListItem(d.Nom, d.Id.ToString())).ToList();
        }

        private void AjouterErreursAuSommaire()
        {
            var erreurs = ModelState
                .Where(x => x.Key != string.Empty && x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                .Distinct()
                .ToList();

            foreach (var erreur in erreurs)
            {
                ModelState.AddModelError(string.Empty, erreur);
            }
        }
    }
}
