using Exercices_Formulaire_Web.Data;
using Exercices_Formulaire_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices_Formulaire_Web.Services
{
    public class EmployesService(ApplicationDbContext context)
    {
        public async Task<List<Employe>> GetAllAsync()
        {
            return await context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Employe employe)
        {
            await context.Employes.AddAsync(employe);
            return await context.SaveChangesAsync();
        }

        public async Task<Employe?> FindAsync(int id)
        {
            return await context.Employes
                .Include(e => e.PaysOrigine)
                .Include(e => e.Departement)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> UpdateAsync(Employe employe)
        {
            context.Employes.Update(employe);
            return await context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(Employe employe)
        {
            context.Employes.Remove(employe);
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(Employe employe)
        {
            context.Employes.Add(employe);
            return await context.SaveChangesAsync();
        }
    }
}
