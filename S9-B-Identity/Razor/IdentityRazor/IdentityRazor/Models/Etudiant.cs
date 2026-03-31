using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityRazor.Models
{
    public class Etudiant
    {
        public int Id { get; set; }

        [Required]
        public string Programme { get; set; } = string.Empty;

        public int NumeroEtudiant { get; set; }

        public DateTime DateInscription { get; set; }

        public double MoyenneGenerale { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }
}
