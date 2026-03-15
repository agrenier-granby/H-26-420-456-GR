using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S4_A_EF.Data;
using S4_A_EF.Models;
using S4_A_EF.ViewModels;

namespace S4_A_EF.Controllers
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
                SalaireAnnuel = employe.SalaireAnnuel
            };

            return View(viewModel);
        }

        // GET: Employes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var employe = new Employe
                {
                    Nom = viewModel.Nom,
                    Age = viewModel.Age,
                    DateEmbauche = viewModel.DateEmbauche,
                    SalaireAnnuel = viewModel.SalaireAnnuel,
                    PaysId = viewModel.PaysId
                };
                _context.Employes.Add(employe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

            var viewModel = new EmployeEditViewModel
            {
                Id = employe.Id,
                Nom = employe.Nom,
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                SalaireAnnuel = employe.SalaireAnnuel,
                PaysId = employe.PaysId
            };

            return View(viewModel);
        }

        // POST: Employes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeEditViewModel viewModel)
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
