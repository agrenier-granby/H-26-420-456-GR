using Exercices.BackgroundTasks.Web.Models;

namespace Exercices.BackgroundTasks.Web.Services
{

    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByBirthdayAsync(DateTime date);
    }
}
