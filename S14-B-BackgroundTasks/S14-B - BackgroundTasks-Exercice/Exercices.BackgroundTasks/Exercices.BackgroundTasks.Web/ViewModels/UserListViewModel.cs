using Exercices.BackgroundTasks.Web.Models;

namespace Exercices.BackgroundTasks.Web.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<User> Users { get; set; } = [];
    }
}
