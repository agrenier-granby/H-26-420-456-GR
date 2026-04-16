using Exercice_Mapper.Models;
using Exercice_Mapper.Services;
using Exercice_Mapper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Mapper.Controllers
{
    public class DepartementsController(DepartementsService service) : Controller
    {
        private readonly DepartementsService _service = service;

        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = _service.GetByIdWithEmployes(id.Value);
            if (departement == null)
            {
                return NotFound();
            }

            return View(new FormulaireDepartementVM()
            {
                DepartementId = departement.DepartementId,
                Budget = departement.Budget,
                Nom = departement.Nom,
                Employes = departement.Employes
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = _service.GetById(id.Value);
            if (departement == null)
            {
                return NotFound();
            }

            return View(new FormulaireDepartementVM()
            {
                DepartementId = departement.DepartementId,
                Budget = departement.Budget,
                Nom = departement.Nom,
                Employes = departement.Employes
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(FormulaireDepartementVM departementVM)
        {
            if (ModelState.IsValid)
            {
                // Nouveau département
                if (departementVM.DepartementId == 0)
                {
                    var departement = new Departement()
                    {
                        Budget = departementVM.Budget,
                        Nom = departementVM.Nom
                    };
                    _service.Add(departement);
                }
                //Sinon, c'est un département existant, on doit alors le mettre a jour dans la BD
                else
                {
                    try
                    {
                        var departementToUpdate = _service.GetById(departementVM.DepartementId);
                        if (departementToUpdate == null)
                        {
                            return NotFound();
                        }

                        departementToUpdate.Budget = departementVM.Budget;
                        departementToUpdate.Nom = departementVM.Nom;

                        _service.Update(departementToUpdate);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_service.Exists(departementVM.DepartementId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), departementVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = _service.GetById(id.Value);
            if (departement == null)
            {
                return NotFound();
            }

            return View(new FormulaireDepartementVM()
            {
                DepartementId = departement.DepartementId,
                Budget = departement.Budget,
                Nom = departement.Nom,
                Employes = departement.Employes
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var departement = _service.GetById(id);
            if (departement == null)
            {
                return NotFound();
            }
            _service.Delete(departement);
            return RedirectToAction(nameof(Index));
        }
    }
}
