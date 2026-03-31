using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityRazor.Models
{
    public class Employe
    {
        public int Id { get; set; }

        [Required]
        public string Poste { get; set; } = string.Empty;

        [Required]
        public string Departement { get; set; } = string.Empty;

        public DateTime DateEmbauche { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
