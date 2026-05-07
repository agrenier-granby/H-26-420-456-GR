using System.ComponentModel.DataAnnotations;

namespace BackgroundTasks.Web.Exemple.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public DateOnly DateOfBirth { get; init; } // init => Can only be set once in the constructor; https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init

        public DateTimeOffset CreatedAt { get; }

        public DateTimeOffset LastSuccessfulLoginAt { get; set; }

    }
}
