namespace BackgroundTasks.Web.Exemple.ViewModels
{
    public class UserVM
    {
        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public DateOnly DateOfBirth { get; init; } // init => Can only be set once in the constructor; https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init
        public Guid Id { get; internal set; }
    }
}
