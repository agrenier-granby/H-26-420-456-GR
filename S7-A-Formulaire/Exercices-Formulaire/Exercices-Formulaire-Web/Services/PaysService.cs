using Exercices_Formulaire_Web.Data;
using Exercices_Formulaire_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Formulaire_Web.Services
{
    public class PaysService(ApplicationDbContext context)
    {

        public async Task<List<Pays>> GetAll()
        {
            return await context.Pays.ToListAsync();
        }
    }
}
