using Exercices_Formulaire_Web.Models;

namespace Exercices_Formulaire_Web.ViewModels
{
    public class EmployeIndexVM
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateNaissance { get; set; }
        public DateTime DateEmbauche { get; set; }
        public float SalaireAnnuel { get; set; }
        public Titre Titre { get; set; }
        public StatutEmploye Statut { get; set; }
        public TypeRemuneration TypeRemuneration { get; set; }
        public string? PaysOrigineNom { get; set; }
        public string? DepartementNom { get; set; }
    }
}
