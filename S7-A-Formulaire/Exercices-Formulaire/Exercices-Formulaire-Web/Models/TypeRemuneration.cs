using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.Models
{
    public enum TypeRemuneration
    {
        [Display(Name = "Taux horaire")]
        TauxHoraire = 0,
        [Display(Name = "Pourboire")]
        Pourboire = 1,
        [Display(Name = "Salaire annuel")]
        Annuel = 2
    }
}
