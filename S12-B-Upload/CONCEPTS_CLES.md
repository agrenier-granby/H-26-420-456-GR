# Concepts clés - Exercice de téléversement d'images

## 1. Architecture en couches

### Pourquoi séparer en couches ?

L'architecture en couches permet de :

- **Séparer les responsabilités** : Chaque couche a un rôle spécifique
- **Faciliter la maintenance** : Modifier une couche sans affecter les autres
- **Améliorer la testabilité** : Tester chaque couche indépendamment
- **Réutiliser le code** : Les services peuvent être utilisés par différents contrôleurs

### Les couches de notre application

```
┌─────────────────────────────────────┐
│         Contrôleurs (Controllers)    │  ← Gère les requêtes HTTP
│         - ImageController            │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│         Services (Business Logic)    │  ← Logique métier
│         - ImageService               │
│         - FileStorageService         │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│         Data Access (EF Core)        │  ← Accès aux données
│         - ApplicationDbContext       │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│      Base de données (LocalDB)       │  ← Stockage persistant
└─────────────────────────────────────┘
```

---

## 2. Injection de dépendances (Dependency Injection)

### Qu'est-ce que c'est ?

L'injection de dépendances est un patron de conception qui permet de :

- Découpler les classes
- Faciliter les tests unitaires
- Gérer automatiquement le cycle de vie des objets

### Comment ça fonctionne ?

#### Étape 1 : Enregistrement dans `Program.cs`

```csharp
builder.Services.AddScoped<IImageService, ImageService>();
```

Cela signifie : "Quand quelqu'un demande un `IImageService`, donne-lui un `ImageService`"

#### Étape 2 : Injection dans le contrôleur

```csharp
public class ImageController : Controller
{
    private readonly IImageService _imageService;

    // ASP.NET Core injecte automatiquement le service
    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }
}
```

### Les différents cycles de vie

| Type          | Description                                 | Utilisation                |
| ------------- | ------------------------------------------- | -------------------------- |
| **Transient** | Une nouvelle instance à chaque demande      | Services légers sans état  |
| **Scoped**    | Une instance par requête HTTP               | Services avec DbContext    |
| **Singleton** | Une seule instance pour toute l'application | Services avec cache global |

Dans notre cas, on utilise **Scoped** car nos services utilisent `ApplicationDbContext`.

---

## 3. Pattern Repository/Service

### Pourquoi utiliser des services ?

❌ **Mauvaise pratique** : Logique métier dans le contrôleur

```csharp
public async Task<IActionResult> Upload(UploadImageViewModel model)
{
    // ❌ Trop de logique dans le contrôleur
    var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");
    Directory.CreateDirectory(uploadFolder);
    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    // ... encore plus de code ...
}
```

✅ **Bonne pratique** : Logique métier dans les services

```csharp
public async Task<IActionResult> Upload(UploadImageViewModel model)
{
    // ✅ Le contrôleur délègue au service
    var filePath = await _fileStorageService.SaveFileAsync(file, "uploads/images");
    await _imageService.CreateAsync(image);
}
```

### Avantages

1. **Réutilisabilité** : Le service peut être utilisé par plusieurs contrôleurs
2. **Testabilité** : Facile de créer des mocks pour les tests
3. **Maintenabilité** : La logique est centralisée
4. **Lisibilité** : Le contrôleur est plus simple à comprendre

---

## 4. Nomenclature des méthodes

### Règle importante : Pas de redondance

❌ **Mauvais**

```csharp
public class ImageService
{
    public Task<List<Image>> GetAllImagesAsync() // "Images" est redondant
    public Task<Image> CreateImageAsync()        // "Image" est redondant
}
```

✅ **Bon**

```csharp
public class ImageService
{
    public Task<List<Image>> GetAllAsync()  // Pas de redondance
    public Task<Image> CreateAsync()         // Plus concis
}
```

Le nom de la classe (`ImageService`) indique déjà qu'on travaille avec des images.

### Convention de nommage

- **Méthodes asynchrones** : Suffixe `Async`
- **Méthodes CRUD** :
  - `GetAllAsync()` - Récupérer tous
  - `GetByIdAsync(int id)` - Récupérer un par ID
  - `CreateAsync(entity)` - Créer
  - `UpdateAsync(entity)` - Mettre à jour
  - `DeleteAsync(int id)` - Supprimer

---

## 5. Entity Framework Core

### Qu'est-ce qu'Entity Framework Core ?

EF Core est un ORM (Object-Relational Mapper) qui permet de :

- Travailler avec une base de données en utilisant des objets C#
- Ne pas écrire de SQL directement (dans la plupart des cas)
- Gérer les migrations de base de données

### Le DbContext

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Image> Images { get; set; }
}
```

- **DbContext** : Représente une session avec la base de données
- **DbSet<Image>** : Représente une table dans la base de données

### Migrations

#### Créer une migration

```bash
dotnet ef migrations add InitialCreate
```

Cela génère du code qui créera la structure de la base de données.

#### Appliquer la migration

```bash
dotnet ef database update
```

Cela exécute les migrations et crée/modifie la base de données.

---

## 6. Vues partielles et AJAX

### Pourquoi utiliser une vue partielle ?

Une vue partielle est un morceau de HTML réutilisable qui peut être :

- Inclus dans plusieurs pages
- Chargé dynamiquement via AJAX

### Dans notre exercice

#### Vue partielle (`_ImageUploadPartial.cshtml`)

```cshtml
<div class="image-upload-item mb-2">
    <input type="file" name="Images" class="form-control" accept="image/*" />
    <button type="button" class="btn btn-outline-danger btn-sm remove-image-btn">
        <i class="bi bi-trash"></i>
    </button>
</div>
```

#### Chargement via AJAX (`image-upload.js`)

```javascript
$.ajax({
  url: "/Image/GetImageUploadPartial",
  type: "GET",
  success: function (data) {
    $("#image-uploads-container").append(data);
  },
});
```

### Avantages

✅ Pas besoin de recharger toute la page
✅ Expérience utilisateur plus fluide
✅ Code HTML réutilisable

---

## 7. Gestion des fichiers

### Téléversement sécurisé

#### Bonnes pratiques appliquées

1. **Noms de fichiers uniques**

```csharp
var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
```

Évite les collisions et les écrasements de fichiers.

2. **Validation du type de fichier**

```html
<input type="file" accept="image/*" />
```

Limite les types de fichiers acceptés.

3. **Stockage hors de la racine de l'application**

```csharp
var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads/images");
```

4. **Chemins relatifs dans la base de données**

```csharp
return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
```

Stocke `uploads/images/fichier.jpg` au lieu du chemin complet.

### Pourquoi ne pas stocker les images dans la base de données ?

| Approche                | Avantages                                                           | Inconvénients                                         |
| ----------------------- | ------------------------------------------------------------------- | ----------------------------------------------------- |
| **Fichiers sur disque** | ✅ Performant<br>✅ Facile à sauvegarder<br>✅ Peut utiliser un CDN | ❌ Nécessite gestion des fichiers orphelins           |
| **Base de données**     | ✅ Transactions ACID<br>✅ Pas de fichiers orphelins                | ❌ Base de données volumineuse<br>❌ Moins performant |

**Recommandation** : Fichiers sur disque + chemin dans la base de données (notre approche)

---

## 8. Sécurité

### Protection CSRF (Cross-Site Request Forgery)

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Upload(UploadImageViewModel model)
```

Le token anti-forgery empêche les attaques CSRF où un site malveillant pourrait soumettre un formulaire à votre nom.

### Validation côté serveur

```csharp
if (!ModelState.IsValid)
{
    return View(model);
}
```

Toujours valider côté serveur, même si la validation côté client existe.

### Limitation de taille

```javascript
if (imageCount >= maxImages) {
  alert("Vous ne pouvez téléverser que 5 images maximum.");
  return;
}
```

Limite le nombre de fichiers pour éviter les abus.

### Configuration des limites de requête (Kestrel)

Par défaut, ASP.NET Core limite la taille des requêtes HTTP pour des raisons de sécurité. Pour le téléversement de fichiers, il faut augmenter ces limites :

```csharp
// Dans Program.cs
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

// Configuration de Kestrel
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

// Configuration des formulaires multipart
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
```

#### Pourquoi c'est important ?

| Problème                    | Symptôme                              | Solution                             |
| --------------------------- | ------------------------------------- | ------------------------------------ |
| **Limite dépassée**         | Application plante sans avertissement | Augmenter `MaxRequestBodySize`       |
| **Formulaire trop grand**   | Erreur 413 (Payload Too Large)        | Augmenter `MultipartBodyLengthLimit` |
| **Différences navigateurs** | Fonctionne sur Chrome mais pas Brave  | Configurer toutes les limites        |

⚠️ **Note de sécurité** : N'augmentez pas ces limites excessivement. Définissez une limite raisonnable selon vos besoins (ex: 50 MB, 100 MB).

---

## 9. Asynchrone (async/await)

### Pourquoi utiliser async/await ?

```csharp
// ❌ Synchrone - bloque le thread
public Image Create(Image image)
{
    _context.Images.Add(image);
    _context.SaveChanges();  // Bloque
    return image;
}

// ✅ Asynchrone - libère le thread
public async Task<Image> CreateAsync(Image image)
{
    _context.Images.Add(image);
    await _context.SaveChangesAsync();  // Ne bloque pas
    return image;
}
```

### Avantages

- **Scalabilité** : Le serveur peut gérer plus de requêtes simultanées
- **Performance** : Utilisation optimale des ressources
- **Responsive** : L'application reste réactive

### Règle importante

⚠️ **async tout le long** : Si une méthode est async, toutes les méthodes qui l'appellent doivent aussi être async.

```
Controller (async) → Service (async) → DbContext (async)
```

---

## 10. JavaScript moderne - Fetch API vs jQuery

### Évolution des pratiques

Dans cette solution, nous utilisons la **Fetch API** native de JavaScript au lieu de jQuery. Voici pourquoi :

### Comparaison

#### Avec jQuery (approche traditionnelle)

```javascript
$(document).ready(function () {
  $("#add-image-btn").click(function () {
    $.ajax({
      url: "/Image/GetImageUploadPartial",
      type: "GET",
      success: function (data) {
        $("#image-uploads-container").append(data);
      },
      error: function () {
        alert("Erreur lors de l'ajout d'un champ d'image.");
      },
    });
  });
});
```

#### Avec Fetch API (approche moderne)

```javascript
document.addEventListener("DOMContentLoaded", function () {
  const addImageBtn = document.getElementById("add-image-btn");

  addImageBtn.addEventListener("click", async function () {
    try {
      const response = await fetch("/Image/GetImageUploadPartial", {
        method: "GET",
        headers: {
          "Content-Type": "text/html",
        },
      });

      if (!response.ok) {
        throw new Error("Erreur lors de la récupération du partial.");
      }

      const data = await response.text();
      imageUploadsContainer.insertAdjacentHTML("beforeend", data);
    } catch (error) {
      console.error("Erreur:", error);
      alert("Erreur lors de l'ajout d'un champ d'image.");
    }
  });
});
```

### Avantages de Fetch API

| Aspect                  | jQuery                               | Fetch API                 |
| ----------------------- | ------------------------------------ | ------------------------- |
| **Dépendances**         | ❌ Nécessite jQuery (~30 KB)         | ✅ Natif (0 KB)           |
| **Performance**         | ❌ Plus lourd                        | ✅ Plus rapide            |
| **Syntaxe**             | Callbacks (`.success()`, `.error()`) | ✅ Promises + async/await |
| **Gestion d'erreurs**   | ❌ Moins flexible                    | ✅ try/catch natif        |
| **Support navigateurs** | ✅ Excellent                         | ✅ Excellent (IE11+)      |
| **Modernité**           | ❌ Technologie ancienne              | ✅ Standard moderne       |

### Manipulation du DOM

**jQuery :**

```javascript
$('#element').click(function() { ... });
$('#container').append(html);
$(this).closest('.item').remove();
```

**JavaScript natif :**

```javascript
document.getElementById('element').addEventListener('click', function() { ... });
document.getElementById('container').insertAdjacentHTML('beforeend', html);
event.target.closest('.item').remove();
```

### Quand utiliser quoi ?

- **Fetch API** : ✅ Projets modernes, performances critiques, pas de dépendances externes
- **jQuery** : Seulement si vous avez déjà jQuery dans votre projet pour d'autres raisons

### Ressources

- [Fetch API - MDN](https://developer.mozilla.org/fr/docs/Web/API/Fetch_API)
- [async/await - MDN](https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Statements/async_function)

---

## 11. Pattern MVC

### Model-View-Controller

```
     ┌──────────┐
     │  Client  │
     └─────┬────┘
           │ HTTP Request
           ↓
     ┌──────────┐
     │Controller│ ← Reçoit la requête
     └─────┬────┘
           │
           ↓
     ┌──────────┐
     │  Model   │ ← Traite les données
     └─────┬────┘
           │
           ↓
     ┌──────────┐
     │   View   │ ← Génère le HTML
     └─────┬────┘
           │ HTTP Response
           ↓
     ┌──────────┐
     │  Client  │
     └──────────┘
```

### Dans notre application

- **Model** : `Image`, `UploadImageViewModel`
- **View** : `Index.cshtml`, `Upload.cshtml`
- **Controller** : `ImageController`

---

## Résumé des concepts appliqués

✅ **Architecture en couches** : Controllers → Services → Data Access
✅ **Injection de dépendances** : Services enregistrés et injectés
✅ **Pattern Repository/Service** : Logique métier dans les services
✅ **Nomenclature** : Méthodes sans redondance, en anglais
✅ **Entity Framework Core** : ORM pour l'accès aux données
✅ **Vues partielles et Fetch API** : Chargement dynamique avec JavaScript moderne
✅ **Gestion des fichiers** : Stockage sécurisé sur disque
✅ **Sécurité** : CSRF, validation serveur, limites de requête
✅ **Asynchrone** : Utilisation de async/await
✅ **Pattern MVC** : Séparation Model-View-Controller
✅ **JavaScript moderne** : Fetch API au lieu de jQuery

---

## Pour aller plus loin

### Améliorations possibles

1. **Validation avancée**
   - Vérifier la taille des fichiers
   - Vérifier le type MIME réel (pas juste l'extension)
   - Vérifier les dimensions de l'image

2. **Optimisation**
   - Générer des miniatures (thumbnails)
   - Compresser les images
   - Utiliser un CDN pour servir les images

3. **Fonctionnalités**
   - Supprimer des images
   - Modifier le titre/description
   - Galerie avec pagination
   - Recherche d'images

4. **Tests**
   - Tests unitaires des services
   - Tests d'intégration du contrôleur
   - Tests de l'interface utilisateur

### Ressources pour approfondir

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Dependency Injection in .NET](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection)
- [File uploads in ASP.NET Core](https://docs.microsoft.com/aspnet/core/mvc/models/file-uploads)
