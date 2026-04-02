using System.ComponentModel.DataAnnotations;

namespace IdentityMVC.Models;

public class Employe
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [Display(Name = "Poste")]
    public string Poste { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Département")]
    public string Departement { get; set; } = string.Empty;

    [Display(Name = "Date d'embauche")]
    public DateTime DateEmbauche { get; set; }
}
