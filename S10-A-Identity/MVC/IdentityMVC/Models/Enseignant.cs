using System.ComponentModel.DataAnnotations;

namespace IdentityMVC.Models;

public class Enseignant
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [Display(Name = "Matière enseignée")]
    public string MatiereEnseignee { get; set; } = string.Empty;

    [Display(Name = "Date d'embauche")]
    public DateTime DateEmbauche { get; set; }

    [Display(Name = "Salaire")]
    public double Salaire { get; set; }
}
