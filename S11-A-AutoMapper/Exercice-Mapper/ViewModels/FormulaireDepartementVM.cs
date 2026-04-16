using Exercice_Mapper.Models;
using System.ComponentModel.DataAnnotations;

namespace Exercice_Mapper.ViewModels
{
    public class FormulaireDepartementVM
    {
        public int DepartementId { get; set; }

        [StringLength(50, ErrorMessage = "Votre nom ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; } = default!;

        public double Budget { get; set; }

        public List<Employe>? Employes { get; set; } = default!;
    }
}