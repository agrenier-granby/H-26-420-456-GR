
using Exercice_Mapper.Models;
using Exercice_Mapper.Services;
using Exercice_Mapper.ViewModels;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Mapper.Controllers
{
    public class EmployesController(EmployesService employesService, PaysService paysService, DepartementsService departementsService, IMapper mapper) : Controller
    {
        private readonly EmployesService _employesService = employesService;
        private readonly PaysService _paysService = paysService;
        private readonly DepartementsService _departementsService = departementsService;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            //var employes = _employesService.GetAll();
            var emp = new Employe()
            {
                EmployeId = 2,
                Nom = "Dupont",
                Age = 30,
                DateEmbauche = DateTime.Now,
                Salaire = 3000,
                EstEnEmploi = true,
                PaysId = 1,
                PaysOrigine = new Pays()
                {
                    PaysId = 1,
                    Nom = "France"
                },
                DepartementId = 1,
                Departement = new Departement()
                {
                    DepartementId = 1,
                    Nom = "Informatique"
                }

            };

            var vm2 = emp.Adapt<EmployeIndexVM>();
            var neasdf = _mapper.Map<Employe, EmployeIndexVM>(emp);
            return View(null);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = _employesService.GetById(id.Value);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        public IActionResult Create()
        {
            EmployeVM vm = new()
            {
                PaysSLI = _paysService.GetSelectListItems(),
                DepartementsSLI = _departementsService.GetSelectListItems()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.PaysSLI = _paysService.GetSelectListItems();
                vm.DepartementsSLI = _departementsService.GetSelectListItems();

                return View(vm);
            }

            // Sans Mapper
            //Employe employeAAjoute = new()
            //{
            //    Nom = vm.Nom,
            //    Age = vm.Age,
            //    DateEmbauche = vm.DateEmbauche,
            //    Salaire = vm.Salaire,
            //    EstEnEmploi = vm.EstEnEmploi,
            //    PaysId = (int)vm.PaysId,
            //    DepartementId = (int)vm.DepartementId
            //};

            // Avec Mapper
            Employe employeAAjoute = _mapper.Map<Employe>(vm);

            _employesService.Add(employeAAjoute);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = _employesService.GetEditVMById(id.Value);
            if (vm == null)
            {
                return NotFound();
            }
            vm.PaysSLI = _paysService.GetSelectListItems();
            vm.DepartementsSLI = _departementsService.GetSelectListItems();
            return View(vm);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeEditVM vm)
        {
            if (id != vm.EmployeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Avec Mapper
                Employe employeAModifie = _mapper.Map<Employe>(vm);
                try
                {
                    _employesService.Update(employeAModifie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_employesService.Exists(employeAModifie.EmployeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Edit), vm);
        }

    }
}
