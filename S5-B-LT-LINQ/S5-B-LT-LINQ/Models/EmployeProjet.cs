namespace S5_B_LT_LINQ.Models
{
    public class EmployeProjet
    {
        public virtual Employe Employe { get; set; } = default!;
        public int EmployeId { get; set; }

        public virtual Projet Projet { get; set; } = default!;
        public int ProjetId { get; set; }

        public DateTime DateAjout { get; set; }
    }
}
