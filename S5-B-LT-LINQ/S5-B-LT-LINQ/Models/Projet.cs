namespace S5_B_LT_LINQ.Models
{
    public class Projet
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public virtual ICollection<Employe> Employes { get; set; } = new List<Employe>();
        public virtual ICollection<EmployeProjet> EmployeProjets { get; set; } = new List<EmployeProjet>();
    }
}
