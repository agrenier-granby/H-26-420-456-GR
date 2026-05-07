using BackgroundTasks.Web.Exemple.Models;
using BackgroundTasks.Web.Exemple.ViewModels;

namespace BackgroundTasks.Web.Exemple.Extensions
{
    public static class UserMapping
    {
        public static User ToUser(this UserVM userVM)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = userVM.Email,
                FullName = userVM.FullName,
                DateOfBirth = userVM.DateOfBirth
            };
        }

        public static UserVM ToUserResponse(this User user)
        {
            return new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth
            };
        }

        public static IEnumerable<UserVM> ToUserResponse(this IEnumerable<User> users)
        {
            return users.Select(ToUserResponse);
        }
    }
}
