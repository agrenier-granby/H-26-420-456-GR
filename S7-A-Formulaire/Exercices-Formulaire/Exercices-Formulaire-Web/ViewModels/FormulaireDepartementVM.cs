using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.ViewModels
{
    public class FormulaireDepartementVM
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public double Budget { get; set; }
    }
}
