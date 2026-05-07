using BackgroundTasks.Web.Exemple.Models;
using BackgroundTasks.Web.Exemple.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace BackgroundTasks.Web.Exemple.Services
{
    public class UserService<T>(UserRepository userRepository) where T : User
    {
        private readonly UserRepository _userRepository = userRepository;
        public async Task<OperationResult<User>> CreateAsync(User user)
        {
            var existingUser = await _userRepository.Exists(user.Id);

            if (existingUser)
            {
                var message = $"A user with id {user.Id} already exists";
                throw new ValidationException(message, GenerateValidationError(message));
            }

            // TODO: Validate if the user Id is updated once saved
            var result = await _userRepository.CreateAsync(user);
            return new OperationResult<User>()
            {
                Success = result > 0,
                Entity = user
            };
        }

        internal async Task<User?> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user;
        }

        internal async Task<IEnumerable<User>> GetAllAsync(int? page = null, int? pageSize = null)
        {
            return await _userRepository.GetAllAsync(page, pageSize);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        private static ValidationFailure[] GenerateValidationError(string message)
        {
            return [new ValidationFailure(nameof(User), message)];
        }

        internal Task<bool> DeleteAsync(Guid id)
        {
            return _userRepository.DeleteAsync(id);
        }
    }
}
