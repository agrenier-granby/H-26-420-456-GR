using Exercice_Mapper.Data;
using Exercice_Mapper.Models;
using Exercice_Mapper.ViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Mapper.Services
{
    public class EmployesService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public List<EmployeIndexVM> GetAll()
        {
            var employes = _context.Employes
                .Include(e => e.Departement)
                .Include(e => e.PaysOrigine);
            return employes.ProjectToType<EmployeIndexVM>().ToList();
        }

        public Employe? GetById(int id)
        {
            return _context.Employes
                .Include(e => e.Departement)
                .Include(e => e.PaysOrigine)
                .FirstOrDefault(m => m.EmployeId == id);
        }

        public EmployeEditVM? GetEditVMById(int id)
        {
            return _context.Employes
                .ProjectToType<EmployeEditVM>()
                .FirstOrDefault(x => x.EmployeId == id);
        }

        public void Add(Employe employe)
        {
            _context.Employes.Add(employe);
            _context.SaveChanges();
        }

        public void Update(Employe employe)
        {
            _context.Update(employe);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Employes.Any(e => e.EmployeId == id);
        }
    }
}
