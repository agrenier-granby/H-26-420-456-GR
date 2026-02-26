using Exercices_Formulaire_Web.Data;
using Exercices_Formulaire_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices_Formulaire_Web.Services
{
    public class DepartementsService(ApplicationDbContext context)
    {
        public async Task<List<Departement>> GetAllAsync()
        {
            return await context.Departements.ToListAsync();
        }

        public async Task<int> AddAsync(Departement departement)
        {
            context.Departements.Add(departement);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Departement departement)
        {
            context.Departements.Update(departement);
            return await context.SaveChangesAsync();
        }

        public async Task<Departement?> FindAsync(int id)
        {
            return await context.Departements.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Departements.AnyAsync(e => e.Id == id);
        }

        public async Task<int> RemoveAsync(Departement departement)
        {
            context.Departements.Remove(departement);
            return await context.SaveChangesAsync();
        }
    }
}
