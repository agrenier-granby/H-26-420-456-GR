using System.ComponentModel.DataAnnotations;

namespace S5_A_SEEDER.Models
{
    public class Departement
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        public double BudgetAnnuel { get; set; }

        public List<Employe> Employes { get; set; } = new List<Employe>();

        public Employe? Directeur { get; set; }
        public int? DirecteurId { get; set; }
    }
}
