using Microsoft.AspNetCore.Mvc;

namespace BackgroundTasks.Web.Exemple.ViewModels
{
    public class UserUpdateVM
    {
        [FromRoute(Name = "id")] public Guid Id { get; init; }

        [FromBody] public UserVM User { get; set; } = default!;
    }
}
