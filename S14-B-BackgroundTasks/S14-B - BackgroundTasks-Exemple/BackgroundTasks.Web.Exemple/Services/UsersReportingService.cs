using BackgroundTasks.Web.Exemple.Data;
using BackgroundTasks.Web.Exemple.Extensions;
using BackgroundTasks.Web.Exemple.ViewModels;

namespace BackgroundTasks.Web.Exemple.Services
{
    public class UsersReportingService(ILogger<UsersReportingService> logger, ApplicationDbContext context)
    {
        private readonly ILogger<UsersReportingService> _logger = logger;
        private readonly ApplicationDbContext _context = context;

        public async Task<ICollection<UserVM>> GenerateReportForNewlyCreatedUsersAsync(DateTime sinceThisDate)
        {
            var newlyCreatedUsers = _context.Users.Where(x => x.CreatedAt <= sinceThisDate).ToList();

            _logger.LogInformation("[{threadId}]-Génération du rapport en cours pour [{count}] utilisateurs...", Thread.CurrentThread.Name, newlyCreatedUsers.Count);

            // Simuler la création du rapport
            await Task.Delay(2000);

            _logger.LogInformation("[{threadId}]-Génération du rapport terminé.", Thread.CurrentThread.Name);
            return newlyCreatedUsers.Select(x => x.ToUserResponse()).ToList();
        }


        public async Task<ICollection<UserVM>> GenerateReportForUsersLastSuccessfulLoginAsync(DateTime sinceThisDate)
        {
            var lastSuccessfulUsers = _context.Users.Where(x => x.LastSuccessfulLoginAt <= sinceThisDate).ToList();

            _logger.LogInformation("[{threadId}]-Génération du rapport en cours pour [{count}] utilisateurs...", Thread.CurrentThread.Name, lastSuccessfulUsers.Count);

            // Simuler la création du rapport
            await Task.Delay(2000);

            _logger.LogInformation("[{threadId}]-Génération du rapport terminé.", Thread.CurrentThread.Name);
            return lastSuccessfulUsers.Select(x => x.ToUserResponse()).ToList();
        }
    }
}
