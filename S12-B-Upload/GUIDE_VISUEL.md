# Guide visuel - Comprendre le flux de téléversement

## 🎯 Vue d'ensemble

Ce document explique visuellement comment fonctionne le téléversement d'images dans l'application.

---

## 📸 Scénario : Téléverser 3 images

Imaginons qu'un utilisateur veut téléverser 3 images avec le titre "Vacances 2024" et la description "Photos de mes vacances".

---

## 🔄 Flux complet étape par étape

### Étape 1 : Navigation vers la page de téléversement

```
Utilisateur clique sur "Images" dans la navigation
    ↓
Browser envoie: GET /Image/Index
    ↓
ImageController.Index() est appelé
    ↓
ImageService.GetAllAsync() récupère les images
    ↓
Vue Index.cshtml affiche la galerie
    ↓
Utilisateur clique sur "Téléverser des images"
    ↓
Browser envoie: GET /Image/Upload
    ↓
ImageController.Upload() retourne la vue
    ↓
Vue Upload.cshtml s'affiche avec 1 champ d'upload
```

---

### Étape 2 : Ajout de 2 champs d'upload supplémentaires

```
Utilisateur clique sur "Ajouter une autre image" (1ère fois)
    ↓
JavaScript (image-upload.js) détecte le clic
    ↓
Vérification: imageCount (1) < maxImages (5) ? OUI
    ↓
$.ajax({
    url: '/Image/GetImageUploadPartial',
    type: 'GET'
})
    ↓
ImageController.GetImageUploadPartial() retourne le HTML
    ↓
JavaScript ajoute le HTML au DOM
    ↓
imageCount = 2
    ↓
[L'utilisateur répète pour obtenir 3 champs au total]
```

**Résultat visuel dans le browser :**

```
┌─────────────────────────────────────────┐
│ Titre: [Vacances 2024              ]   │
├─────────────────────────────────────────┤
│ Description:                            │
│ [Photos de mes vacances            ]   │
├─────────────────────────────────────────┤
│ Images:                                 │
│ [Choisir un fichier] 🗑️                │
│ [Choisir un fichier] 🗑️                │
│ [Choisir un fichier] 🗑️                │
│ [+ Ajouter une autre image]            │
└─────────────────────────────────────────┘
```

---

### Étape 3 : Sélection des images

```
Utilisateur clique sur "Choisir un fichier" (3 fois)
    ↓
Sélectionne:
    - photo1.jpg
    - photo2.jpg
    - photo3.jpg
```

**État du formulaire :**

```
Title = "Vacances 2024"
Description = "Photos de mes vacances"
Images[0] = photo1.jpg (IFormFile)
Images[1] = photo2.jpg (IFormFile)
Images[2] = photo3.jpg (IFormFile)
```

---

### Étape 4 : Soumission du formulaire

```
Utilisateur clique sur "Téléverser"
    ↓
Browser envoie: POST /Image/Upload
    Content-Type: multipart/form-data
    Body:
        - Title: "Vacances 2024"
        - Description: "Photos de mes vacances"
        - Images: [photo1.jpg, photo2.jpg, photo3.jpg]
    ↓
ASP.NET Core Model Binding crée UploadImageViewModel
    ↓
ImageController.Upload(model) est appelé
```

---

### Étape 5 : Validation du modèle

```
ImageController.Upload(model)
    ↓
if (!ModelState.IsValid) ❓
    ├─ NON → return View(model) [Réaffiche avec erreurs]
    └─ OUI → Continue
    ↓
if (model.Images == null || !model.Images.Any()) ❓
    ├─ OUI → ModelState.AddModelError() → return View(model)
    └─ NON → Continue
```

---

### Étape 6 : Traitement de chaque image (Boucle)

```
foreach (var file in model.Images)  // 3 itérations
{
    ┌─────────────────────────────────────────────────────────┐
    │ Itération 1 : photo1.jpg                                │
    ├─────────────────────────────────────────────────────────┤
    │ 1. Appel FileStorageService.SaveFileAsync()             │
    │    ↓                                                     │
    │    Génère: 8d5f2a3c-4b6e-...._photo1.jpg               │
    │    ↓                                                     │
    │    Crée: wwwroot/uploads/images/                        │
    │    ↓                                                     │
    │    Copie le fichier                                     │
    │    ↓                                                     │
    │    Retourne: "uploads/images/8d5f2a3c-..._photo1.jpg"  │
    │                                                          │
    │ 2. Création de l'objet Image                            │
    │    var image = new Image {                              │
    │        Title = "Vacances 2024",                         │
    │        Description = "Photos de mes vacances",          │
    │        FilePath = "uploads/images/8d5f2a3c-..._photo1.jpg" │
    │    };                                                    │
    │                                                          │
    │ 3. Appel ImageService.CreateAsync(image)                │
    │    ↓                                                     │
    │    image.UploadedAt = DateTime.Now                      │
    │    ↓                                                     │
    │    _context.Images.Add(image)                           │
    │    ↓                                                     │
    │    await _context.SaveChangesAsync()                    │
    │    ↓                                                     │
    │    INSERT INTO Images (Title, Description, FilePath, UploadedAt) │
    │    VALUES ('Vacances 2024', 'Photos...', 'uploads/...', '2024-...') │
    └─────────────────────────────────────────────────────────┘

    [Répète pour photo2.jpg et photo3.jpg]
}
```

---

### Étape 7 : État final

**Dans le système de fichiers :**

```
wwwroot/
└── uploads/
    └── images/
        ├── 8d5f2a3c-4b6e-4a9d-8f7e-1234567890ab_photo1.jpg
        ├── 9e6g3b4d-5c7f-5b0e-9g8f-2345678901bc_photo2.jpg
        └── 0f7h4c5e-6d8g-6c1f-0h9g-3456789012cd_photo3.jpg
```

**Dans la base de données (Table Images) :**

```
┌────┬──────────────┬────────────────────┬────────────────────────────────────┬─────────────────────┐
│ Id │ Title        │ Description        │ FilePath                           │ UploadedAt          │
├────┼──────────────┼────────────────────┼────────────────────────────────────┼─────────────────────┤
│ 1  │ Vacances 2024│ Photos de mes...  │ uploads/images/8d5f2a3c-..._ph...  │ 2024-01-15 14:30:00 │
│ 2  │ Vacances 2024│ Photos de mes...  │ uploads/images/9e6g3b4d-..._ph...  │ 2024-01-15 14:30:01 │
│ 3  │ Vacances 2024│ Photos de mes...  │ uploads/images/0f7h4c5e-..._ph...  │ 2024-01-15 14:30:02 │
└────┴──────────────┴────────────────────┴────────────────────────────────────┴─────────────────────┘
```

---

### Étape 8 : Redirection et affichage

```
ImageController.Upload() termine
    ↓
return RedirectToAction(nameof(Index))
    ↓
Browser envoie: GET /Image/Index
    ↓
ImageController.Index()
    ↓
ImageService.GetAllAsync()
    ↓
SELECT * FROM Images ORDER BY UploadedAt DESC
    ↓
Retourne List<Image> avec 3 images
    ↓
Vue Index.cshtml reçoit le modèle
    ↓
foreach (var image in Model)
    Affiche chaque image dans une carte Bootstrap
    <img src="~/uploads/images/8d5f2a3c-..._photo1.jpg" />
```

**Résultat visuel :**

```
┌─────────────────────────────────────────────────────┐
│             Galerie d'images                         │
├─────────────────────────────────────────────────────┤
│  ┌─────────┐  ┌─────────┐  ┌─────────┐            │
│  │ [Photo] │  │ [Photo] │  │ [Photo] │            │
│  │ Vacances│  │ Vacances│  │ Vacances│            │
│  │  2024   │  │  2024   │  │  2024   │            │
│  │ Photos  │  │ Photos  │  │ Photos  │            │
│  │ de mes  │  │ de mes  │  │ de mes  │            │
│  │ vacances│  │ vacances│  │ vacances│            │
│  └─────────┘  └─────────┘  └─────────┘            │
└─────────────────────────────────────────────────────┘
```

---

## 🔍 Zoom sur les composants clés

### A. FileStorageService.SaveFileAsync()

```
┌──────────────────────────────────────────────────────┐
│ SaveFileAsync(IFormFile file, string folder)         │
├──────────────────────────────────────────────────────┤
│                                                       │
│ file.FileName = "photo1.jpg"                         │
│          ↓                                            │
│ Guid.NewGuid() = "8d5f2a3c-4b6e-4a9d-8f7e-123..."   │
│          ↓                                            │
│ uniqueFileName = "8d5f2a3c-..._photo1.jpg"          │
│          ↓                                            │
│ uploadFolder = "C:\...\wwwroot\uploads\images"      │
│          ↓                                            │
│ Directory.CreateDirectory(uploadFolder)              │
│          ↓                                            │
│ filePath = "C:\...\wwwroot\uploads\images\8d5f..."  │
│          ↓                                            │
│ using (var fileStream = new FileStream(...))         │
│ {                                                     │
│     await file.CopyToAsync(fileStream);              │
│ }                                                     │
│          ↓                                            │
│ return "uploads/images/8d5f2a3c-..._photo1.jpg"     │
│                                                       │
└──────────────────────────────────────────────────────┘
```

### B. ImageService.CreateAsync()

```
┌──────────────────────────────────────────────────────┐
│ CreateAsync(Image image)                             │
├──────────────────────────────────────────────────────┤
│                                                       │
│ image.UploadedAt = DateTime.Now                      │
│          ↓                                            │
│ _context.Images.Add(image)                           │
│          ↓                                            │
│ await _context.SaveChangesAsync()                    │
│          ↓                                            │
│ Entity Framework génère:                             │
│                                                       │
│ INSERT INTO Images                                   │
│   (Title, Description, FilePath, UploadedAt)         │
│ VALUES                                               │
│   (@p0, @p1, @p2, @p3)                              │
│                                                       │
│ Paramètres:                                          │
│   @p0 = "Vacances 2024"                             │
│   @p1 = "Photos de mes vacances"                    │
│   @p2 = "uploads/images/8d5f2a3c-..._photo1.jpg"   │
│   @p3 = "2024-01-15 14:30:00"                       │
│          ↓                                            │
│ return image (avec Id généré par la BD)              │
│                                                       │
└──────────────────────────────────────────────────────┘
```

### C. Injection de dépendances

```
┌────────────────────────────────────────────────────┐
│ Quand l'application démarre (Program.cs)           │
├────────────────────────────────────────────────────┤
│                                                     │
│ builder.Services.AddScoped<IImageService,          │
│                             ImageService>();        │
│                                                     │
│ ASP.NET Core enregistre:                           │
│   "Pour IImageService, crée une instance           │
│    de ImageService par requête HTTP"               │
│                                                     │
└────────────────────────────────────────────────────┘
         ↓
┌────────────────────────────────────────────────────┐
│ Quand une requête arrive à ImageController         │
├────────────────────────────────────────────────────┤
│                                                     │
│ ASP.NET Core regarde le constructeur:              │
│                                                     │
│ public ImageController(                            │
│     IImageService imageService,          ← Besoin │
│     IFileStorageService fileStorageService) ← Besoin│
│                                                     │
│         ↓                                           │
│                                                     │
│ ASP.NET Core crée automatiquement:                 │
│   1. new ApplicationDbContext(options)             │
│   2. new ImageService(context)                     │
│   3. new FileStorageService(environment)           │
│   4. new ImageController(imageService,             │
│                           fileStorageService)       │
│                                                     │
└────────────────────────────────────────────────────┘
```

---

## 🎨 Rendu des vues

### Upload.cshtml - Génération du HTML

```razor
@model UploadImageViewModel

<input asp-for="Title" class="form-control" />
```

**Se transforme en :**

```html
<input type="text" id="Title" name="Title" value="" class="form-control" />
```

**Quand soumis, ASP.NET Core fait automatiquement :**

```csharp
var model = new UploadImageViewModel();
model.Title = Request.Form["Title"];
model.Description = Request.Form["Description"];
// Magie du Model Binding!
```

---

## 🔐 Protection CSRF expliquée

### Sans protection CSRF (DANGER) :

```
Site malveillant (evil.com)
    ↓
<form action="https://votresite.com/Image/Upload" method="POST">
    <input type="hidden" name="Title" value="Image piratée">
    <input type="submit" value="Cliquez ici pour gagner!">
</form>
    ↓
Si l'utilisateur est connecté à votresite.com
et clique sur le bouton...
    ↓
Formulaire soumis avec les cookies de l'utilisateur!
    ↓
💥 Attaque réussie!
```

### Avec protection CSRF (SÉCURISÉ) :

```
votresite.com génère un token unique par session
    ↓
<form>
    <input type="hidden" name="__RequestVerificationToken"
           value="CfDJ8Abc123..." />
    ↓
Quand le formulaire est soumis:
    ↓
ASP.NET Core vérifie:
  - Le token dans le formulaire
  - Le token dans le cookie
  - Correspondent-ils?
    ├─ OUI → Traite la requête
    └─ NON → Rejette (400 Bad Request)
    ↓
Le site malveillant ne peut PAS:
  - Connaître le token
  - Générer le bon token
    ↓
✅ Attaque bloquée!
```

---

## 📊 Diagramme de séquence complet

```
┌─────────┐  ┌──────────┐  ┌─────────────┐  ┌──────────┐  ┌──────┐
│Browser  │  │Controller│  │ImageService │  │FileStorage│  │  DB  │
└────┬────┘  └────┬─────┘  └──────┬──────┘  └─────┬────┘  └───┬──┘
     │            │                │                │            │
     │ POST Upload│                │                │            │
     │───────────>│                │                │            │
     │            │                │                │            │
     │            │ [Foreach image]│                │            │
     │            │                │                │            │
     │            │ SaveFileAsync()│                │            │
     │            │────────────────┼───────────────>│            │
     │            │                │                │            │
     │            │                │         [Copie fichier]     │
     │            │                │                │            │
     │            │ filePath       │                │            │
     │            │<───────────────┼────────────────│            │
     │            │                │                │            │
     │            │ CreateAsync()  │                │            │
     │            │───────────────>│                │            │
     │            │                │                │            │
     │            │                │    INSERT INTO Images       │
     │            │                │───────────────────────────>│
     │            │                │                │            │
     │            │                │    Image       │            │
     │            │                │<───────────────────────────│
     │            │                │                │            │
     │            │ Image          │                │            │
     │            │<───────────────│                │            │
     │            │                │                │            │
     │            │ [Fin foreach]  │                │            │
     │            │                │                │            │
     │ Redirect   │                │                │            │
     │<───────────│                │                │            │
     │            │                │                │            │
     │ GET Index  │                │                │            │
     │───────────>│                │                │            │
     │            │                │                │            │
     │            │ GetAllAsync()  │                │            │
     │            │───────────────>│                │            │
     │            │                │                │            │
     │            │                │    SELECT * FROM Images     │
     │            │                │───────────────────────────>│
     │            │                │                │            │
     │            │                │    List<Image> │            │
     │            │                │<───────────────────────────│
     │            │                │                │            │
     │            │ List<Image>    │                │            │
     │            │<───────────────│                │            │
     │            │                │                │            │
     │ HTML       │                │                │            │
     │<───────────│                │                │            │
     │            │                │                │            │
```

---

## 🎯 Points clés à retenir

1. **Le contrôleur orchestre** : Il ne fait pas le travail, il délègue
2. **Les services travaillent** : Ils contiennent toute la logique métier
3. **L'injection de dépendances connecte** : Elle crée et fournit les instances
4. **Entity Framework traduit** : C# ↔ SQL automatiquement
5. **Les vues affichent** : Elles génèrent du HTML à partir des données

---

## 🔄 Async/Await visualisé

### Synchrone (BLOQUE) :

```
Thread 1 : [Request A──────SaveFile──────SaveDB────]
Thread 2 : [           Request B────────────────────]
Thread 3 : [                  Request C─────────────]

❌ Beaucoup de threads nécessaires
❌ Attentes inutiles
```

### Asynchrone (LIBÈRE) :

```
Thread 1 : [Request A─┐        ┌─Complete]
           [         await      ]
           [Request B─┐  ┌─Complete       ]
           [         await]
           [Request C──┐ ┌──Complete      ]

✅ Moins de threads
✅ Meilleure utilisation des ressources
```

---

Ce guide visuel devrait vous aider à comprendre exactement comment tout fonctionne ensemble ! 🎉
