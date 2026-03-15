using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using S5_B_LT_LINQ.Data;
using S5_B_LT_LINQ.Models;
using S5_B_LT_LINQ.ViewModels;

namespace S5_B_LT_LINQ.Controllers
{
    public class EmployesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employes
        public async Task<IActionResult> Index()
        {
            var employes = await _context.Employes.ToListAsync();
            var viewModel = employes.Select(e => new EmployeIndexViewModel
            {
                Id = e.Id,
                Nom = e.Nom,
                Age = e.Age,
                DateEmbauche = e.DateEmbauche,
                SalaireAnnuel = e.SalaireAnnuel
            }).ToList();

            return View(viewModel);
        }

        // GET: Employes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employe == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeDetailsViewModel
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                PaysOrigine = employe.PaysOrigine?.Nom,
                Departement = employe.Departement?.Nom
            };

            return View(viewModel);
        }

        // GET: Employes/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EmployeFormViewModel
            {
                DateEmbauche = DateTime.Today,
                PaysList = await _context.Pays
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nom })
                    .ToListAsync(),
                DepartementsList = await _context.Departements
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Employes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Nom = viewModel.Nom,
                    Age = viewModel.Age,
                    DateEmbauche = viewModel.DateEmbauche,
                    SalaireAnnuel = viewModel.SalaireAnnuel,
                    PaysId = viewModel.PaysId,
                    DepartementId = viewModel.DepartementId
                };

                _context.Employes.Add(employe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.PaysList = await _context.Pays
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nom })
                .ToListAsync();
            viewModel.DepartementsList = await _context.Departements
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Employes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeFormViewModel
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                PaysId = employe.PaysId,
                DepartementId = employe.DepartementId,
                PaysList = await _context.Pays
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nom })
                    .ToListAsync(),
                DepartementsList = await _context.Departements
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Employes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employe = await _context.Employes.FindAsync(id);
                    if (employe == null)
                    {
                        return NotFound();
                    }

                    employe.Nom = viewModel.Nom;
                    employe.Age = viewModel.Age;
                    employe.DateEmbauche = viewModel.DateEmbauche;
                    employe.SalaireAnnuel = viewModel.SalaireAnnuel;
                    employe.PaysId = viewModel.PaysId;
                    employe.DepartementId = viewModel.DepartementId;

                    _context.Update(employe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeExists(viewModel.Id))
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

            viewModel.PaysList = await _context.Pays
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nom })
                .ToListAsync();
            viewModel.DepartementsList = await _context.Departements
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Employes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employe == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeDeleteViewModel
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel
            };

            return View(viewModel);
        }

        // POST: Employes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employe = await _context.Employes.FindAsync(id);
            if (employe != null)
            {
                _context.Employes.Remove(employe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(int id)
        {
            return _context.Employes.Any(e => e.Id == id);
        }
    }
}
