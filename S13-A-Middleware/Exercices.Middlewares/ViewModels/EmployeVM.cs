using Exercices.Middlewares.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exercices.Middlewares.ViewModels
{
    public class EmployeVM
    {
        public List<Pays> Pays { get; set; } = default!;

        // Pour le Tag Helper
        public List<SelectListItem> PaysSLI { get; set; } = default!;

        public List<Departement> Departements { get; set; } = default!;

        [Required(ErrorMessage = "Le champ " + nameof(Nom) + " est requis")]
        public string Nom { get; set; } = default!;

        [Range(18, 75, ErrorMessage = "L'âge doit être entre 18 et 75 ans.")]
        public int Age { get; set; }

        [Display(Name = "Date d'embauche")]
        public DateTime DateEmbauche { get; set; } = DateTime.Now.Date;             // Pour éviter de commencer au tout début

        public int Salaire { get; set; }

        [Display(Name = "Présentement à l'emploi")]
        public bool EstEnEmploi { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [Display(Name = "Pays d'origine")]
        public int? PaysId { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [Display(Name = "Département de travail")]
        public int? DepartementId { get; set; }
    }
}
