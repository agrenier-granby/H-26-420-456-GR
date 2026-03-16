using Exercices_Formulaire_Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.ViewModels
{
    public class EmployeEditVM : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100)]
        [RegularExpression(@".*\S.*", ErrorMessage = "Le nom ne peut pas contenir uniquement des espaces.")]
        public string? Nom { get; set; }

        [Display(Name = "Âge")]
        [Range(18, 75)]
        public int? Age { get; set; }

        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date de naissance est requise.")]
        [DateDansLePasse(ErrorMessage = "La date de naissance doit être dans le passé.")]
        public DateTime? DateNaissance { get; set; }

        [Display(Name = "Date d'embauche")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date d'embauche est requise.")]
        public DateTime? DateEmbauche { get; set; }

        [Display(Name = "Date de départ")]
        [DataType(DataType.Date)]
        public DateTime? DateDepart { get; set; }

        [Display(Name = "Salaire annuel")]
        [Range(40000, 120000, ErrorMessage = "Le salaire annuel doit être entre 40 000$ et 120 000$.")]
        public float? SalaireAnnuel { get; set; }

        [Display(Name = "Titre")]
        [Required(ErrorMessage = "Le titre est requis.")]
        public Titre? Titre { get; set; }

        [Display(Name = "Statut")]
        public StatutEmploye Statut { get; set; }

        [Display(Name = "Type de rémunération")]
        public TypeRemuneration TypeRemuneration { get; set; }

        public List<SelectListItem> ListePays { get; set; } = new();
        public List<SelectListItem> ListeDepartements { get; set; } = new();

        [Display(Name = "Pays d'origine")]
        public int PaysId { get; set; }

        [Display(Name = "Équipe")]
        [Range(1, int.MaxValue, ErrorMessage = "L'équipe est requise.")]
        public int DepartementId { get; set; }

        public Pays? PaysOrigine { get; set; }
        public Departement? Departement { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateNaissance.HasValue && DateEmbauche.HasValue)
            {
                var date18Ans = DateNaissance.Value.Date.AddYears(18);
                if (date18Ans > DateEmbauche.Value.Date)
                {
                    yield return new ValidationResult(
                        "L'employé doit avoir au moins 18 ans à sa date d'embauche.",
                        new[] { nameof(DateNaissance), nameof(DateEmbauche) });
                }
            }

            if (Titre.HasValue && SalaireAnnuel.HasValue)
            {
                var salaireMinimum = Titre.Value switch
                {
                    Models.Titre.Testeur => 40000,
                    Models.Titre.Developpeur => 60000,
                    Models.Titre.ChefDEquipe => 80000,
                    Models.Titre.GestionnaireDeProjet => 100000,
                    _ => 40000
                };

                if (SalaireAnnuel.Value < salaireMinimum)
                {
                    yield return new ValidationResult(
                        $"Le salaire minimum pour le titre sélectionné est {salaireMinimum:N0}$.",
                        new[] { nameof(SalaireAnnuel), nameof(Titre) });
                }
            }
        }
    }
}
