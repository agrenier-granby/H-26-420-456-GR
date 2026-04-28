using Exercices.Middlewares.Models;

namespace Exercices.Middlewares.ViewModels
{
    public class EmployeEditVM
    {
        public List<Pays> Pays { get; set; } = default!;

        public List<Departement> Departements { get; set; } = default!;

        public Employe Employe { get; set; } = default!;
    }
}
