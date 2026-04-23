# Exercice : Téléversement d'images avec ASP.NET Core MVC

## Objectif

Créer une fonctionnalité complète permettant aux utilisateurs de téléverser plusieurs images (jusqu'à 5) avec un titre et une description. Les images seront enregistrées sur le serveur et leurs chemins seront stockés dans une base de données.

## Compétences visées

- Manipulation de formulaires avec fichiers
- Injection de dépendances
- Utilisation d'Entity Framework Core
- Vues partielles et AJAX
- JavaScript pour l'interface dynamique

---

## Étape 1 : Installation des packages NuGet nécessaires

Ouvrez le terminal dans Visual Studio et exécutez les commandes suivantes :

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

---

## Étape 2 : Créer le modèle de données

### 2.1 Créer le modèle `Image`

Créez un nouveau fichier `Models/Image.cs` :

```csharp
namespace Exercices.UploadFiles.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
```

### 2.2 Créer le modèle de vue `UploadImageViewModel`

Créez un nouveau fichier `Models/UploadImageViewModel.cs` :

```csharp
using Microsoft.AspNetCore.Http;

namespace Exercices.UploadFiles.Models
{
    public class UploadImageViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = new();
    }
}
```

---

## Étape 3 : Créer le contexte de base de données

Créez un nouveau dossier `Data` et un fichier `Data/ApplicationDbContext.cs` :

```csharp
using Exercices.UploadFiles.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.UploadFiles.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
```

---

## Étape 4 : Créer la classe de service

### 4.1 Créer l'interface `IImageService`

Créez un nouveau dossier `Services` et un fichier `Services/IImageService.cs` :

```csharp
using Exercices.UploadFiles.Models;

namespace Exercices.UploadFiles.Services
{
    public interface IImageService
    {
        Task<List<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task<Image> CreateAsync(Image image);
        Task<bool> DeleteAsync(int id);
    }
}
```

### 4.2 Créer l'implémentation `ImageService`

Créez un fichier `Services/ImageService.cs` :

```csharp
using Exercices.UploadFiles.Data;
using Exercices.UploadFiles.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.UploadFiles.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images.OrderByDescending(i => i.UploadedAt).ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<Image> CreateAsync(Image image)
        {
            image.UploadedAt = DateTime.Now;
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
                return false;

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

---

## Étape 5 : Créer le service de gestion de fichiers

### 5.1 Créer l'interface `IFileStorageService`

Créez un fichier `Services/IFileStorageService.cs` :

```csharp
namespace Exercices.UploadFiles.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        bool DeleteFile(string filePath);
    }
}
```

### 5.2 Créer l'implémentation `FileStorageService`

Créez un fichier `Services/FileStorageService.cs` :

```csharp
namespace Exercices.UploadFiles.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Le fichier est vide ou invalide.");

            var uploadFolder = Path.Combine(_environment.WebRootPath, folder);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
        }

        public bool DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}
```

---

## Étape 6 : Configurer les services dans Program.cs

Modifiez le fichier `Program.cs` pour ajouter les services :

```csharp
using Exercices.UploadFiles.Data;
using Exercices.UploadFiles.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration de la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injection de dépendances des services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
```

---

## Étape 7 : Ajouter la chaîne de connexion

Modifiez le fichier `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ExercicesUploadFilesDb;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## Étape 8 : Créer le contrôleur

Créez un nouveau fichier `Controllers/ImageController.cs` :

```csharp
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
                    var filePath = await _fileStorageService.SaveFileAsync(file, "uploads/images");

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
```

---

## Étape 9 : Créer les vues

### 9.1 Créer le dossier Views/Image

Créez un nouveau dossier `Views/Image`.

### 9.2 Créer la vue Index

Créez un fichier `Views/Image/Index.cshtml` :

```cshtml
@model List<Exercices.UploadFiles.Models.Image>

@{
    ViewData["Title"] = "Liste des images";
}

<div class="container">
    <div class="row mb-4">
        <div class="col">
            <h1>Galerie d'images</h1>
        </div>
        <div class="col text-end">
            <a asp-action="Upload" class="btn btn-primary">
                <i class="bi bi-upload"></i> Téléverser des images
            </a>
        </div>
    </div>

    @if (!Model.Any())
    {
        <div class="alert alert-info">
            Aucune image téléversée pour le moment.
        </div>
    }
    else
    {
        <div class="row row-cols-1 row-cols-md-3 g-4">
            @foreach (var image in Model)
            {
                <div class="col">
                    <div class="card h-100">
                        <img src="~/@image.FilePath" class="card-img-top" alt="@image.Title" style="height: 200px; object-fit: cover;">
                        <div class="card-body">
                            <h5 class="card-title">@image.Title</h5>
                            <p class="card-text">@image.Description</p>
                            <small class="text-muted">Téléversé le @image.UploadedAt.ToString("dd/MM/yyyy à HH:mm")</small>
                        </div>
                    </div>
                </div>
            }
        </div>
    }
</div>
```

### 9.3 Créer la vue Upload

Créez un fichier `Views/Image/Upload.cshtml` :

```cshtml
@model Exercices.UploadFiles.Models.UploadImageViewModel

@{
    ViewData["Title"] = "Téléverser des images";
}

<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <h1 class="mb-4">Téléverser des images</h1>

            <form asp-action="Upload" method="post" enctype="multipart/form-data">
                <div class="card">
                    <div class="card-body">
                        <div class="mb-3">
                            <label asp-for="Title" class="form-label">Titre</label>
                            <input asp-for="Title" class="form-control" required />
                            <span asp-validation-for="Title" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Description" class="form-label">Description</label>
                            <textarea asp-for="Description" class="form-control" rows="3"></textarea>
                            <span asp-validation-for="Description" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Images</label>
                            <div id="image-uploads-container">
                                <div class="image-upload-item mb-2">
                                    <input type="file" name="Images" class="form-control" accept="image/*" required />
                                </div>
                            </div>
                            <button type="button" id="add-image-btn" class="btn btn-outline-secondary btn-sm mt-2">
                                <i class="bi bi-plus-circle"></i> Ajouter une autre image
                            </button>
                            <small class="form-text text-muted d-block mt-1">
                                Vous pouvez téléverser jusqu'à 5 images.
                            </small>
                        </div>

                        <div class="d-flex justify-content-between">
                            <a asp-action="Index" class="btn btn-secondary">Annuler</a>
                            <button type="submit" class="btn btn-primary">Téléverser</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script src="~/js/image-upload.js"></script>
}
```

### 9.4 Créer la vue partielle

Créez un fichier `Views/Image/_ImageUploadPartial.cshtml` :

```cshtml
<div class="image-upload-item mb-2">
    <div class="input-group">
        <input type="file" name="Images" class="form-control" accept="image/*" />
        <button type="button" class="btn btn-outline-danger btn-sm remove-image-btn">
            <i class="bi bi-trash"></i>
        </button>
    </div>
</div>
```

---

## Étape 10 : Créer le fichier JavaScript

Créez un fichier `wwwroot/js/image-upload.js` :

```javascript
$(document).ready(function () {
  let imageCount = 1;
  const maxImages = 5;

  $("#add-image-btn").click(function () {
    if (imageCount >= maxImages) {
      alert("Vous ne pouvez téléverser que 5 images maximum.");
      return;
    }

    $.ajax({
      url: "/Image/GetImageUploadPartial",
      type: "GET",
      success: function (data) {
        $("#image-uploads-container").append(data);
        imageCount++;

        if (imageCount >= maxImages) {
          $("#add-image-btn").prop("disabled", true);
        }
      },
      error: function () {
        alert("Erreur lors de l'ajout d'un champ d'image.");
      },
    });
  });

  $(document).on("click", ".remove-image-btn", function () {
    $(this).closest(".image-upload-item").remove();
    imageCount--;

    if (imageCount < maxImages) {
      $("#add-image-btn").prop("disabled", false);
    }
  });
});
```

---

## Étape 11 : Mettre à jour la navigation

Modifiez le fichier `Views/Shared/_Layout.cshtml` pour ajouter un lien vers la nouvelle page dans la barre de navigation :

Ajoutez cet élément `<li>` dans la liste `<ul class="navbar-nav flex-grow-1">` :

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-controller="Image" asp-action="Index">Images</a>
</li>
```

---

## Étape 12 : Créer et appliquer la migration

Dans le terminal, exécutez les commandes suivantes :

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Étape 13 : Tester l'application

1. Lancez l'application (F5)
2. Cliquez sur le lien "Images" dans la barre de navigation
3. Cliquez sur "Téléverser des images"
4. Remplissez le formulaire :
   - Entrez un titre
   - Entrez une description
   - Sélectionnez une image
   - Cliquez sur "Ajouter une autre image" pour téléverser plus d'images (jusqu'à 5)
5. Cliquez sur "Téléverser"
6. Vérifiez que les images apparaissent dans la galerie

---

## Points importants à retenir

### Architecture en couches

- **Contrôleur** : Gère les requêtes HTTP, ne contient pas de logique métier
- **Services** : Contiennent toute la logique métier et les interactions avec la BD
- **Repository pattern** : Via Entity Framework Core pour l'accès aux données

### Injection de dépendances

- Les services sont enregistrés dans `Program.cs` avec `AddScoped`
- Les dépendances sont injectées via le constructeur
- Facilite les tests unitaires et la maintenance

### Nomenclature des méthodes

- Méthodes en anglais dans les services
- Pas de redondance : `ImageService.GetAllAsync()` au lieu de `ImageService.GetAllImagesAsync()`
- Suffixe `Async` pour les méthodes asynchrones

### Sécurité

- Utilisation de `[ValidateAntiForgeryToken]` pour prévenir les attaques CSRF
- Validation des fichiers téléversés
- Noms de fichiers uniques (GUID) pour éviter les collisions

### Performance

- Utilisation de méthodes asynchrones (`async/await`)
- Chargement dynamique des champs d'upload via AJAX
- Utilisation de vues partielles pour optimiser le rendu

---

## Améliorations possibles

1. **Validation des types de fichiers** : Vérifier que seules les images sont acceptées
2. **Limitation de taille** : Limiter la taille des fichiers téléversés
3. **Miniatures** : Générer des miniatures pour optimiser l'affichage
4. **Suppression d'images** : Ajouter une fonctionnalité pour supprimer des images
5. **Pagination** : Paginer la liste des images si elle devient trop longue
6. **Prévisualisation** : Afficher une prévisualisation des images avant téléversement

---

## Ressources supplémentaires

- [Documentation ASP.NET Core MVC](https://docs.microsoft.com/aspnet/core/mvc/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Upload de fichiers dans ASP.NET Core](https://docs.microsoft.com/aspnet/core/mvc/models/file-uploads)
- [Injection de dépendances](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection)
