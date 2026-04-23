using Exercices.UploadFiles.Models;
using Exercices.UploadFiles.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercices.UploadFiles.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IFileStorageService _fileStorageService;

        public ImageController(IImageService imageService, IFileStorageService fileStorageService)
        {
            _imageService = imageService;
            _fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var images = await _imageService.GetAllAsync();
            return View(images);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Images == null || !model.Images.Any())
            {
                ModelState.AddModelError("", "Veuillez sélectionner au moins une image.");
                return View(model);
            }

            foreach (var file in model.Images)
            {
                if (file.Length > 0)
                {
                    var filePath = await _fileStorageService.SaveFileAsync(file, "uploads/antoine");

                    var image = new Image
                    {
                        Title = model.Title,
                        Description = model.Description,
                        FilePath = filePath
                    };

                    await _imageService.CreateAsync(image);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetImageUploadPartial()
        {
            return PartialView("_ImageUploadPartial");
        }
    }
}
