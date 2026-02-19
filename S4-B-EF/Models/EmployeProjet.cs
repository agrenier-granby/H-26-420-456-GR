namespace S4_B_EF.Models
{
    public class EmployeProjet
    {
        public Employe Employe { get; set; } = default!;
        public int EmployeId { get; set; }

        public Projet Projet { get; set; } = default!;
        public int ProjetId { get; set; }

        public DateTime DateAjout { get; set; }
    }
}
