using BackgroundTasks.Web.Exemple.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTasks.Web.Exemple.Controllers
{
    public class ReportsController(ILogger<ReportsController> logger, UsersReportingService usersReportService) : Controller
    {
        private readonly ILogger<ReportsController> _logger = logger;
        private readonly UsersReportingService _usersReportService = usersReportService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReportSinceStartOfTheDay()
        {
            _logger.LogInformation("Début de la génération du rapport des utilisateurs créés depuis le début de la journée");
            var users = await _usersReportService.GenerateReportForNewlyCreatedUsersAsync(DateTime.Today);

            _logger.LogInformation("Fin de la génération du rapport");
            // TODO: Créer une vue pour montrer les nouveaux utilisateurs.
            return Json(users);
        }


        [HttpPost]
        public async Task<IActionResult> GenerateLastLoginReportSinceStartOfTheDay()
        {
            _logger.LogInformation("Début de la génération du rapport des utilisateurs créés depuis le début de la journée");
            var users = await _usersReportService.GenerateReportForNewlyCreatedUsersAsync(DateTime.Today);

            _logger.LogInformation("Fin de la génération du rapport");
            // TODO: Créer une vue pour montrer les utilisateurs s'étant connecté.
            return Json(users);
        }
    }
}
