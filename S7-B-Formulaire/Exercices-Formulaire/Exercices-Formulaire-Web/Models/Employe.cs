using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.Models
{
    public class Employe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = default!;

        [Range(18, 75)]
        public int Age { get; set; }

        public DateTime DateNaissance { get; set; }

        public DateTime DateEmbauche { get; set; }

        public DateTime? DateDepart { get; set; }

        [Range(40000, 120000)]
        public float SalaireAnnuel { get; set; }

        public Titre Titre { get; set; }

        public StatutEmploye Statut { get; set; }

        public TypeRemuneration TypeRemuneration { get; set; }

        public int PaysId { get; set; }
        public Pays? PaysOrigine { get; set; } = default!;

        public int DepartementId { get; set; }
        public Departement? Departement { get; set; } = default!;
    }
}
