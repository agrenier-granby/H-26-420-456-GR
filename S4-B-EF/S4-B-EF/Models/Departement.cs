using System.ComponentModel.DataAnnotations;

namespace S4_B_EF.Models
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
