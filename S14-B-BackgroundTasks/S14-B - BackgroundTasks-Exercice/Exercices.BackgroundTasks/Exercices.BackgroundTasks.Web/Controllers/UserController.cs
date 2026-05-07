using Exercices.BackgroundTasks.Web.Services;
using Exercices.BackgroundTasks.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exercices.BackgroundTasks.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        public IUserService Service => _service;

        public async Task<IActionResult> Index()
        {
            var vm = new UserListViewModel
            {
                Users = await Service.GetAllAsync()
            };

            return View(vm);
        }
    }

}
