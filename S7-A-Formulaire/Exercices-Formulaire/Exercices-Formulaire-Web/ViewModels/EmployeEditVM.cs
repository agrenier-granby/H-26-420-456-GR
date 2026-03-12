using Exercices_Formulaire_Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exercices_Formulaire_Web.ViewModels
{
    public class EmployeEditVM
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; } = DateTime.Now;
        public float SalaireAnnuel { get; set; }
        public StatutEmploye Statut { get; set; }
        public TypeRemuneration TypeRemuneration { get; set; }
        public List<SelectListItem> ListePays { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListeDepartements { get; set; } = new List<SelectListItem>();
        public int PaysId { get; set; }
        public int DepartementId { get; set; }
        public Pays? PaysOrigine { get; set; }
        public Departement? Departement { get; set; }
    }
}
