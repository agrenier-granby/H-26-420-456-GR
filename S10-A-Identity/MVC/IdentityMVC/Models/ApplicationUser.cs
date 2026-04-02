using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityMVC.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Surnom { get; set; } = string.Empty;

    [Required]
    public string Nom { get; set; } = string.Empty;

    public string? Adresse { get; set; }
}
