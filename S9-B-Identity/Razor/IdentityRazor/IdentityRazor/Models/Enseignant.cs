using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityRazor.Models
{
    public class Enseignant
    {
        public int Id { get; set; }

        [Required]
        public string MatiereEnseignee { get; set; } = string.Empty;

        public DateTime DateEmbauche { get; set; }

        public decimal Salaire { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
