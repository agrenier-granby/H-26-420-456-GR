using Microsoft.AspNetCore.Mvc;
using S4_A_MVC.Models;
using S4_A_MVC.ViewModels;

namespace S4_A_MVC.Controllers
{
    public class TasksController : Controller
    {
        private static IList<TaskItem> _tasks = new List<TaskItem>();
        private static int _nextId = 1;

        [HttpGet]
        public IActionResult Index(string? category)
        {
            ViewData["IndexViewData"] = "Données de ViewData depuis Index";
            ViewBag.IndexViewBag = "Données de ViewBag depuis Index";
            TempData["IndexTempData"] = "Données de TempData depuis Index";

            var tasks = string.IsNullOrWhiteSpace(category)
                ? _tasks.ToList()
                : _tasks.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            var viewModel = new TasksIndexVM
            {
                Tasks = tasks,
                Category = category
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CreateViewData"] = "Données de ViewData depuis Create (GET)";
            ViewBag.CreateViewBag = "Données de ViewBag depuis Create (GET)";
            TempData["CreateTempData"] = "Données de TempData depuis Create (GET)";

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTaskVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError(nameof(model.Title), "Le titre est requis");
                
                ViewData["CreateViewData"] = "Données de ViewData depuis Create (POST)";
                ViewBag.CreateViewBag = "Données de ViewBag depuis Create (POST)";
                TempData["CreateTempData"] = "Données de TempData depuis Create (POST)";
                
                return View(model);
            }

            var task = new TaskItem
            {
                Id = _nextId++,
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                IsCompleted = false
            };

            _tasks.Add(task);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Complete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
