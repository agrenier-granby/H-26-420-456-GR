using Exercices.BackgroundTasks.Web.Data;
using Exercices.BackgroundTasks.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.BackgroundTasks.Web.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _context.Users.ToListAsync();

    public async Task<IEnumerable<User>> GetByBirthdayAsync(DateTime date)
        => await _context.Users
            .Where(u => u.BirthDate.Month == date.Month &&
                        u.BirthDate.Day == date.Day)
            .ToListAsync();
}
