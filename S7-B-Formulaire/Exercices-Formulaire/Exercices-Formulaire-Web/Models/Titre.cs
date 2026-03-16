using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.Models
{
    public enum Titre
    {
        [Display(Name = "Gestionnaire de projet")]
        GestionnaireDeProjet = 0,
        [Display(Name = "Chef d'équipe")]
        ChefDEquipe = 1,
        [Display(Name = "Développeur")]
        Developpeur = 2,
        [Display(Name = "Testeur")]
        Testeur = 3
    }
}
