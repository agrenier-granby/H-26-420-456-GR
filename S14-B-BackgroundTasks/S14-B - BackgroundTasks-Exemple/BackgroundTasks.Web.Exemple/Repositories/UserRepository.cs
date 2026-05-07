using BackgroundTasks.Web.Exemple.Data;
using BackgroundTasks.Web.Exemple.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTasks.Web.Exemple.Repositories
{
    // TODO: Change the returned values to a Result object containing the user and the status of the operation
    public class UserRepository(ApplicationDbContext dbContext)
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        internal Task<int> CreateAsync(User user)
        {
            _dbContext.Add(user);
            return _dbContext.SaveChangesAsync();
        }

        internal async Task<bool> Exists(Guid id)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == id);
        }

        internal async Task<User?> GetAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        // Avec pagination
        internal async Task<IEnumerable<User>> GetAllAsync(int? page = null, int? pageSize = null)
        {
            var query = _dbContext.Users.AsNoTracking();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await GetAsync(id);
            if (user is null)
            {
                return false;
            }

            _dbContext.Remove(user);
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

    }
}
