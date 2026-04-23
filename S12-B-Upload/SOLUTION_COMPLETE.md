# Solution complète - Téléversement d'images

## 📋 Résumé de la solution

Cette solution implémente une fonctionnalité complète de téléversement d'images multiple avec les caractéristiques suivantes :

### ✨ Fonctionnalités implémentées

- ✅ Formulaire de téléversement avec titre et description
- ✅ Téléversement de 1 à 5 images par soumission
- ✅ Ajout dynamique de champs d'upload via AJAX
- ✅ Stockage des images sur le serveur (dossier `wwwroot/uploads/images`)
- ✅ Enregistrement des métadonnées en base de données LocalDB
- ✅ Galerie d'affichage des images téléversées
- ✅ Navigation intégrée dans la barre de menu
- ✅ Architecture en couches avec injection de dépendances

---

## 📁 Fichiers créés

### Modèles (Models)

- `Models/Image.cs` - Entité pour la base de données
- `Models/UploadImageViewModel.cs` - ViewModel pour le formulaire

### Contrôleurs (Controllers)

- `Controllers/ImageController.cs` - Gestion des requêtes HTTP

### Services

- `Services/IImageService.cs` - Interface du service d'images
- `Services/ImageService.cs` - Logique métier pour les images
- `Services/IFileStorageService.cs` - Interface du service de fichiers
- `Services/FileStorageService.cs` - Gestion du stockage des fichiers

### Accès aux données (Data)

- `Data/ApplicationDbContext.cs` - Contexte Entity Framework Core

### Vues (Views)

- `Views/Image/Index.cshtml` - Galerie d'images
- `Views/Image/Upload.cshtml` - Formulaire de téléversement
- `Views/Image/_ImageUploadPartial.cshtml` - Vue partielle pour AJAX

### JavaScript

- `wwwroot/js/image-upload.js` - Gestion de l'ajout dynamique d'images

### Configuration

- `Program.cs` - Configuration des services, Kestrel et limites de requête (modifié)
- `appsettings.json` - Chaîne de connexion (modifié)
- `Views/Shared/_Layout.cshtml` - Navigation et Bootstrap Icons (modifié)
- `WebApplication1.csproj` - Packages NuGet (modifié)

### Documentation

- `EXERCICE_TELEVERSER_IMAGES.md` - Instructions complètes pour l'étudiant
- `README_QUICK_START.md` - Guide de démarrage rapide
- `CONCEPTS_CLES.md` - Explication des concepts clés

---

## 🚀 Démarrage rapide

### 1. Restaurer les packages

```bash
dotnet restore
```

### 2. Créer la base de données

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3. Lancer l'application

```bash
dotnet run
```

Ou appuyez sur **F5** dans Visual Studio.

---

## 🏗️ Architecture de la solution

```
┌────────────────────────────────────────────────────────────┐
│                    Présentation Layer                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │ Controllers  │  │    Views     │  │  JavaScript  │     │
│  │  - Image     │  │  - Index     │  │  - Upload    │     │
│  │  - Home      │  │  - Upload    │  │              │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
└────────────────────────────────────────────────────────────┘
                            ↓
┌────────────────────────────────────────────────────────────┐
│                     Business Layer                          │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │  ImageService    │  │ FileStorageService│               │
│  │  - GetAllAsync   │  │  - SaveFileAsync  │               │
│  │  - CreateAsync   │  │  - DeleteFile     │               │
│  └──────────────────┘  └──────────────────┘               │
└────────────────────────────────────────────────────────────┘
                            ↓
┌────────────────────────────────────────────────────────────┐
│                    Data Access Layer                        │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │  DBContext       │  │  File System     │               │
│  │  - Images        │  │  - wwwroot/      │               │
│  └──────────────────┘  └──────────────────┘               │
└────────────────────────────────────────────────────────────┘
                            ↓
┌────────────────────────────────────────────────────────────┐
│                      Storage Layer                          │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │  LocalDB         │  │  Image Files     │               │
│  │  (Local Catalog/)│  │  (uploads/)      │               │
│  └──────────────────┘  └──────────────────┘               │
└────────────────────────────────────────────────────────────┘
```

---

## 🎯 Principes appliqués

### 1. Séparation des préoccupations (Separation of Concerns)

- **Contrôleurs** : Gèrent uniquement les requêtes HTTP
- **Services** : Contiennent toute la logique métier
- **Data Access** : Gère l'accès aux données

### 2. Injection de dépendances

```csharp
// Enregistrement
builder.Services.AddScoped<IImageService, ImageService>();

// Injection
public ImageController(IImageService imageService)
{
    _imageService = imageService;
}
```

### 3. Programmation orientée interface

```csharp
// ✅ Dépend de l'interface, pas de l'implémentation
private readonly IImageService _imageService;

// ❌ Ne jamais faire ça
private readonly ImageService _imageService;
```

### 4. Single Responsibility Principle (SRP)

Chaque classe a une seule responsabilité :

- `ImageController` : Gérer les requêtes HTTP
- `ImageService` : Logique métier des images
- `FileStorageService` : Gestion du stockage des fichiers
- `ApplicationDbContext` : Accès aux données

### 5. Nomenclature cohérente

- Méthodes en anglais
- Pas de redondance dans les noms
- Suffixe `Async` pour les méthodes asynchrones

---

## 🔄 Flux de données

### Téléversement d'images

```
1. Utilisateur soumet le formulaire
   └─> Upload.cshtml

2. Requête POST vers le contrôleur
   └─> ImageController.Upload(model)

3. Validation du modèle
   └─> ModelState.IsValid

4. Sauvegarde de chaque fichier
   └─> FileStorageService.SaveFileAsync()
       - Génère un nom unique
       - Crée le dossier si nécessaire
       - Copie le fichier
       - Retourne le chemin relatif

5. Création de l'entité Image
   └─> ImageService.CreateAsync()
       - Ajoute la date de téléversement
       - Enregistre dans la base de données

6. Redirection vers la galerie
   └─> RedirectToAction(nameof(Index))
```

### Affichage de la galerie

```
1. Requête GET vers /Image/Index
   └─> ImageController.Index()

2. Récupération des images
   └─> ImageService.GetAllAsync()
       - Requête à la base de données
       - Tri par date décroissante

3. Rendu de la vue
   └─> Index.cshtml
       - Affiche chaque image dans une carte
       - Utilise le chemin relatif pour les images
```

### Ajout dynamique d'un champ d'upload

```
1. Clic sur "Ajouter une autre image"
   └─> image-upload.js (événement click)

2. Vérification du nombre maximum
   └─> if (imageCount >= maxImages)

3. Requête avec Fetch API
   └─> fetch('/Image/GetImageUploadPartial')
       └─> ImageController.GetImageUploadPartial()
           └─> Retourne _ImageUploadPartial.cshtml

4. Ajout du HTML au DOM
   └─> imageUploadsContainer.insertAdjacentHTML('beforeend', data)

5. Incrémentation du compteur
   └─> imageCount++
```

---

## 🛠️ Technologies et packages

### Framework

- **ASP.NET Core 10.0** - Framework web moderne et performant

### Base de données

- **Entity Framework Core** - ORM pour .NET
- **LocalDB** - Base de données SQL Server local

### Front-end

- **Bootstrap 5** - Framework CSS responsive
- **Bootstrap Icons** - Bibliothèque d'icônes (CDN)
- **Fetch API** - API native pour les requêtes HTTP (remplace jQuery AJAX)
- **Razor Pages** - Moteur de templates

### Packages NuGet

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
```

---

## 📊 Schéma de base de données

```sql
Table: Images
┌────────────────┬─────────────┬──────────┬─────────────┐
│ Column         │ Type        │ Nullable │ Description │
├────────────────┼─────────────┼──────────┼─────────────┤
│ Id             │ INTEGER     │ No       │ Clé primaire│
│ Title          │ TEXT        │ No       │ Titre       │
│ Description    │ TEXT        │ No       │ Description │
│ FilePath       │ TEXT        │ No       │ Chemin      │
│ UploadedAt     │ DATETIME    │ No       │ Date upload │
└────────────────┴─────────────┴──────────┴─────────────┘
```

---

## 🔒 Sécurité implémentée

### 1. Protection CSRF

```csharp
[ValidateAntiForgeryToken]
public async Task<IActionResult> Upload(UploadImageViewModel model)
```

### 2. Validation des fichiers

```html
<input type="file" accept="image/*" required />
```

### 3. Noms de fichiers sécurisés

```csharp
var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
```

### 4. Validation côté serveur

```csharp
if (!ModelState.IsValid)
    return View(model);
```

### 5. Limitation du nombre de fichiers

```javascript
const maxImages = 5;
if (imageCount >= maxImages) return;
```

### 6. Configuration des limites de requête (Kestrel)

```csharp
// Dans Program.cs
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
```

> **📝 Note importante** : Cette configuration est essentielle pour éviter les plantages lors du téléversement avec certains navigateurs (comme Brave), qui gèrent les requêtes différemment de Chrome. Sans ces limites, l'application peut se fermer sans avertissement.

---

## 💻 Code JavaScript moderne

### Fetch API vs jQuery

Cette solution utilise la **Fetch API** native de JavaScript au lieu de jQuery pour les requêtes AJAX. Voici pourquoi :

#### Avantages de Fetch API

- ✅ API native du navigateur (pas de bibliothèque externe)
- ✅ Syntaxe moderne avec async/await
- ✅ Plus performant et léger
- ✅ Meilleure gestion des erreurs avec try/catch
- ✅ Retour de Promises standardisé

#### Comparaison

**Avec jQuery (ancienne méthode) :**

```javascript
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
```

**Avec Fetch API (méthode moderne) :**

```javascript
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
```

#### Manipulation du DOM

**jQuery :**

```javascript
$('#add-image-btn').click(function () { ... });
$('#image-uploads-container').append(data);
```

**JavaScript natif :**

```javascript
document.getElementById('add-image-btn').addEventListener('click', async function () { ... });
imageUploadsContainer.insertAdjacentHTML('beforeend', data);
```

---

## 🎨 UI/UX - Bootstrap Icons

### Intégration

Le fichier `_Layout.cshtml` inclut Bootstrap Icons via CDN :

```html
<link
  rel="stylesheet"
  href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.1/font/bootstrap-icons.min.css"
/>
```

### Utilisation dans les vues

```html
<!-- Bouton avec icône -->
<button
  type="button"
  id="add-image-btn"
  class="btn btn-outline-secondary btn-sm mt-2"
>
  <i class="bi bi-plus-circle"></i> Ajouter une autre image
</button>
```

### Icônes disponibles

Vous pouvez utiliser n'importe quelle icône de la bibliothèque Bootstrap Icons :

- `bi bi-plus-circle` - Ajouter
- `bi bi-trash` - Supprimer
- `bi bi-upload` - Téléverser
- `bi bi-image` - Image
- Et plus de 1,800 autres icônes !

📖 **Documentation complète** : [https://icons.getbootstrap.com/](https://icons.getbootstrap.com/)

---

## ✅ Checklist de validation

Avant de soumettre l'exercice, vérifiez que :

- [ ] L'application compile sans erreurs
- [ ] La base de données est créée (`images.db` existe)
- [ ] Le dossier `wwwroot/uploads/images` existe
- [ ] La navigation contient le lien "Images"
- [ ] Le formulaire de téléversement s'affiche
- [ ] On peut téléverser une image
- [ ] On peut ajouter jusqu'à 5 champs d'upload
- [ ] Les images s'affichent dans la galerie
- [ ] Le titre et la description sont enregistrés
- [ ] Les services utilisent l'injection de dépendances
- [ ] Les noms de méthodes sont en anglais sans redondance
- [ ] Toutes les méthodes d'accès aux données sont asynchrones

---

## 📝 Notes pour l'enseignant

### Critères d'évaluation suggérés

#### Architecture (30%)

- [ ] Séparation correcte en couches
- [ ] Utilisation appropriée de l'injection de dépendances
- [ ] Interfaces définies et implémentées

#### Fonctionnalités (30%)

- [ ] Téléversement d'images fonctionnel
- [ ] Ajout dynamique de champs d'upload
- [ ] Affichage de la galerie

#### Code (25%)

- [ ] Nomenclature correcte (anglais, pas de redondance)
- [ ] Utilisation de async/await
- [ ] Code propre et lisible

#### Sécurité (15%)

- [ ] Validation côté serveur
- [ ] Protection CSRF
- [ ] Gestion sécurisée des fichiers

### Points d'amélioration possibles

1. **Validation avancée**
   - Vérifier la taille des fichiers (ex: max 5MB)
   - Vérifier le type MIME réel
   - Ajouter des messages d'erreur personnalisés

2. **Gestion des erreurs**
   - Try-catch dans les services
   - Pages d'erreur personnalisées
   - Logging des erreurs

3. **Tests**
   - Tests unitaires des services
   - Tests d'intégration du contrôleur

4. **Performance**
   - Génération de miniatures
   - Compression des images
   - Pagination de la galerie

---

## 🎓 Objectifs pédagogiques atteints

✅ Comprendre l'architecture en couches
✅ Maîtriser l'injection de dépendances
✅ Utiliser Entity Framework Core
✅ Manipuler des fichiers en ASP.NET Core
✅ Créer des vues partielles et utiliser AJAX
✅ Appliquer les bonnes pratiques de nomenclature
✅ Implémenter des fonctionnalités de sécurité de base
✅ Utiliser la programmation asynchrone

---

## 📚 Ressources complémentaires

- [EXERCICE_TELEVERSER_IMAGES.md](EXERCICE_TELEVERSER_IMAGES.md) - Instructions détaillées
- [README_QUICK_START.md](README_QUICK_START.md) - Guide de démarrage
- [CONCEPTS_CLES.md](CONCEPTS_CLES.md) - Explication des concepts

---

## 📧 Support

Pour toute question sur l'exercice :

1. Consultez d'abord la documentation fournie
2. Vérifiez les logs de compilation et d'exécution
3. Utilisez les outils de débogage de Visual Studio

---

**Date de création** : 2024
**Version** : 1.0
**Framework** : ASP.NET Core 10.0
