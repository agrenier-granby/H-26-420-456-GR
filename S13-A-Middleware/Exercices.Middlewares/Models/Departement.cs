using System.ComponentModel.DataAnnotations;

namespace Exercices.Middlewares.Models
{
    public class Departement
    {
        public int DepartementId { get; set; }

        [StringLength(50, ErrorMessage = "Votre nom ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; } = default!;

        public double Budget { get; set; }

        public List<Employe> Employes { get; set; } = default!;
    }
}