using Exercice_Mapper.Data;
using Exercice_Mapper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Mapper.Services
{
    public class DepartementsService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public List<SelectListItem> GetSelectListItems()
        {
            return _context.Departements
                .Select(x => new SelectListItem() { Text = x.Nom, Value = x.DepartementId.ToString() })
                .ToList();
        }

        public List<Departement> GetAll()
        {
            return _context.Departements.ToList();
        }

        public Departement? GetById(int id)
        {
            return _context.Departements.Find(id);
        }

        public Departement? GetByIdWithEmployes(int id)
        {
            return _context.Departements
                .Include(x => x.Employes)
                .FirstOrDefault(m => m.DepartementId == id);
        }

        public void Add(Departement departement)
        {
            _context.Departements.Add(departement);
            _context.SaveChanges();
        }

        public void Update(Departement departement)
        {
            _context.Update(departement);
            _context.SaveChanges();
        }

        public void Delete(Departement departement)
        {
            _context.Departements.Remove(departement);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Departements.Any(e => e.DepartementId == id);
        }
    }
}
