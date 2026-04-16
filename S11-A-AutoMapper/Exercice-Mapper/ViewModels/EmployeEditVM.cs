using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exercice_Mapper.ViewModels
{
    public class EmployeEditVM
    {
        // Pour le HTML Helper
        //public List<Pays> Pays { get; set; } = default!;

        // Pour le HTML Helper
        //public List<Departement> Departements { get; set; } = default!;

        // Pour le Tag Helper
        public List<SelectListItem>? PaysSLI { get; set; } = default!;
        public int PaysId { get; set; }

        // Pour le Tag Helper
        public List<SelectListItem>? DepartementsSLI { get; set; } = default!;
        public int DepartementId { get; set; }

        public int EmployeId { get; set; }

        [Required]
        public string Nom { get; set; } = default!;

        [Range(18, 75, ErrorMessage = "La plage doit être entre {0} et {1}")]
        public int Age { get; set; }

        [Required]
        public DateTime DateEmbauche { get; set; }

        [Required]
        public int Salaire { get; set; }

        [Required]
        public bool EstEnEmploi { get; set; }
    }
}
