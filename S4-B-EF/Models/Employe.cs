namespace S4_B_EF.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public int PaysId { get; set; }
        public Pays? PaysOrigine { get; set; }

        public Departement? Departement { get; set; }
        public int DepartementId { get; set; }

        public ICollection<Projet> ProjetsImpliques { get; set; } = new List<Projet>();
        public ICollection<EmployeProjet> EmployeProjets { get; set; } = new List<EmployeProjet>();
    }
}
