using Exercice_Mapper.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exercice_Mapper.Services
{
    public class PaysService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public List<SelectListItem> GetSelectListItems()
        {
            return _context.Pays
                .Select(x => new SelectListItem() { Text = x.Nom, Value = x.PaysId.ToString() })
                .ToList();
        }
    }
}
