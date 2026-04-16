using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exercice_Mapper.ViewModels
{
    public class EmployeVM
    {
        // Pour le Tag Helper
        public List<SelectListItem> PaysSLI { get; set; } = default!;

        public List<SelectListItem> DepartementsSLI { get; set; } = default!;

        [Required(ErrorMessage = "Le champ " + nameof(Nom) + " est requis")]
        public string Nom { get; set; } = default!;

        [Range(18, 75, ErrorMessage = "L'âge doit être entre 18 et 75 ans.")]
        public int Age { get; set; }

        [Display(Name = "Date d'embauche")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.DateTime)]
        public DateTime DateEmbauche { get; set; } = DateTime.Now.Date;             // Pour éviter de commencer au tout début

        [DataType(DataType.Currency)]
        public int Salaire { get; set; }

        [Display(Name = "Présentement à l'emploi")]
        public bool EstEnEmploi { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [Display(Name = "Pays d'origine")]
        public int PaysId { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [Display(Name = "Département de travail")]
        public int DepartementId { get; set; }
    }
}
