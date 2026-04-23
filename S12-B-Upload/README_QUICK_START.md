# Guide de démarrage rapide - Téléversement d'images

## Installation et lancement

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

## Accès à l'application

Une fois l'application lancée :

1. Cliquez sur **"Images"** dans la barre de navigation
2. Cliquez sur **"Téléverser des images"**
3. Remplissez le formulaire et téléversez vos images

## Structure du projet

```
WebApplication1/
├── Controllers/
│   ├── HomeController.cs
│   └── ImageController.cs          ← Gestion des images
├── Data/
│   └── ApplicationDbContext.cs     ← Contexte EF Core
├── Models/
│   ├── ErrorViewModel.cs
│   ├── Image.cs                    ← Modèle de données
│   └── UploadImageViewModel.cs     ← ViewModel pour le formulaire
├── Services/
│   ├── IImageService.cs            ← Interface du service d'images
│   ├── ImageService.cs             ← Implémentation service d'images
│   ├── IFileStorageService.cs      ← Interface du service de fichiers
│   └── FileStorageService.cs       ← Implémentation service de fichiers
├── Views/
│   ├── Home/
│   ├── Image/
│   │   ├── Index.cshtml            ← Galerie d'images
│   │   ├── Upload.cshtml           ← Formulaire de téléversement
│   │   └── _ImageUploadPartial.cshtml  ← Vue partielle pour AJAX
│   └── Shared/
│       └── _Layout.cshtml
└── wwwroot/
    ├── js/
    │   └── image-upload.js         ← JavaScript pour l'ajout dynamique
    └── uploads/
        └── images/                 ← Dossier de stockage des images
```

## Fonctionnalités

✅ Téléversement multiple d'images (jusqu'à 5)
✅ Ajout dynamique de champs d'upload via AJAX
✅ Stockage des images sur le serveur
✅ Enregistrement des métadonnées en base de données
✅ Architecture en couches avec injection de dépendances
✅ Galerie d'affichage des images

## Technologies utilisées

- **ASP.NET Core 10.0** - Framework web
- **Entity Framework Core** - ORM
- **LocalDB** - Base de données
- **Bootstrap 5** - Framework CSS
- **Bootstrap Icons** - Bibliothèque d'icônes (CDN)
- **Fetch API** - API native pour les requêtes HTTP

## Commandes Entity Framework utiles

### Créer une nouvelle migration

```bash
dotnet ef migrations add NomDeLaMigration
```

### Appliquer les migrations

```bash
dotnet ef database update
```

### Supprimer la dernière migration

```bash
dotnet ef migrations remove
```

### Voir les migrations appliquées

```bash
dotnet ef migrations list
```
