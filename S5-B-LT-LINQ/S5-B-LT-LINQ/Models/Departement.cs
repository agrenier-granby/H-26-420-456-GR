using System.ComponentModel.DataAnnotations;

namespace S5_B_LT_LINQ.Models
{
    public class Departement
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        public double BudgetAnnuel { get; set; }

        public virtual List<Employe> Employes { get; set; } = new List<Employe>();

        public virtual Employe? Directeur { get; set; }
        public int? DirecteurId { get; set; }
    }
}
