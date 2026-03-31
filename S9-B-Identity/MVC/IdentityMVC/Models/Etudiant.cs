using System.ComponentModel.DataAnnotations;

namespace IdentityMVC.Models;

public class Etudiant
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [Display(Name = "Programme")]
    public string Programme { get; set; } = string.Empty;

    [Display(Name = "Numéro étudiant")]
    public int NumeroEtudiant { get; set; }

    [Display(Name = "Date d'inscription")]
    public DateTime DateInscription { get; set; }

    [Display(Name = "Moyenne générale")]
    public double MoyenneGenerale { get; set; }
}
