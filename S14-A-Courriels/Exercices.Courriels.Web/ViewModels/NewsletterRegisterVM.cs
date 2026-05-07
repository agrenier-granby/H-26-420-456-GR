using System.ComponentModel.DataAnnotations;

namespace Exercices.Courriels.Web.ViewModels
{
    public class NewsletterRegisterVM
    {
        [Required]
        public string Email { get; set; } = default!;
    }
}
