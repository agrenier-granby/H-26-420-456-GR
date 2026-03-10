using Exercices_Formulaire_Web.Models;

namespace Exercices_Formulaire_Web.ViewModels
{
    public class EmployeCreateVM
    {
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; } = DateTime.Now;
        public float SalaireAnnuel { get; set; }
        public bool EstEnEmploi { get; set; }
        public List<Pays> Pays { get; set; } = new List<Pays>();
        public List<Departement> Departements { get; set; } = new List<Departement>();
        public int PaysId { get; set; }
        public int DepartementId { get; set; }
        public Pays? PaysOrigine { get; set; }
        public Departement? Departement { get; set; }
    }
}
