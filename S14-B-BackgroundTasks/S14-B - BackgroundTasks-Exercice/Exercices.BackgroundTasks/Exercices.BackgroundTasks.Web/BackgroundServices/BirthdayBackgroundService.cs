using Exercices.BackgroundTasks.Web.Services;

namespace Exercices.BackgroundTasks.Web.BackgroundServices
{
    public class BirthdayBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<BirthdayBackgroundService> _logger;

        public BirthdayBackgroundService(IServiceProvider provider,
                                         ILogger<BirthdayBackgroundService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BirthdayBackgroundService démarré.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _provider.CreateScope();
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                    var today = DateTime.Today;
                    var birthdays = await userService.GetByBirthdayAsync(today);

                    foreach (var user in birthdays)
                    {
                        _logger.LogInformation(
                            "Anniversaire aujourd’hui : {FirstName} {LastName} ({BirthDate})",
                            user.FirstName, user.LastName, user.BirthDate.ToShortDateString());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur dans BirthdayBackgroundService");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}