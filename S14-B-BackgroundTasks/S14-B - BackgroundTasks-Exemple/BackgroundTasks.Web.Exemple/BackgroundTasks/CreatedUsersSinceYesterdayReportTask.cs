using BackgroundTasks.Web.Exemple.Services;

namespace BackgroundTasks.Web.Exemple.BackgroundTasks
{
    public class CreatedUsersSinceYesterdayReportTask : IHostedService
    {
        private int _executionCount = 0;
        private readonly ILogger<CreatedUsersSinceYesterdayReportTask> _logger;
        private readonly UsersReportingService _reporting;
        private Timer? _timer = null;

        public CreatedUsersSinceYesterdayReportTask(ILogger<CreatedUsersSinceYesterdayReportTask> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            using var scope = serviceProvider.CreateScope(); // this will use `IServiceScopeFactory` internally

            // Il n'est pas possible d'utiliser le Dependancy Injection dans un IHostedService. On doit accéder à nos services "manuellement" avec l'appel à la classe ServiceProvider.
            _reporting = scope.ServiceProvider.GetRequiredService<UsersReportingService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Classname} Service running.", nameof(CreatedUsersSinceYesterdayReportTask));
            var now = DateTime.Now;
            var tomorrow = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
            var secondsUntilMidnight = tomorrow.Subtract(now).Seconds;

            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(secondsUntilMidnight),
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            _logger.LogInformation("{Classname} Service is working. Generating report for date [{Date:yyyy-MM-dd}], Number of executions since the server is up: {Count}", nameof(CreatedUsersSinceYesterdayReportTask), DateTime.Now, count);

            // Executer l'appel de génération de rapport depuis le service pour obtenir le rapport des utilisateurs créés depuis 24h
            await _reporting.GenerateReportForNewlyCreatedUsersAsync(DateTime.Today.AddDays(-1));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Classname} Service is stopping.", nameof(CreatedUsersSinceYesterdayReportTask));

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

    }
}
