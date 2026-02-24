# Exercice - Loading Types (Eager Loading vs Lazy Loading)

## Actions créées

### 1. EagerLoading (`/Home/EagerLoading`)
- Récupère les 100 premiers employés
- Utilise `Include()` pour charger le pays d'origine et le département (Eager Loading)
- Toutes les données sont chargées en UNE SEULE requête SQL

### 2. LazyLoading (`/Home/LazyLoading`)
- Récupère les 100 premiers employés
- N'utilise PAS `Include()` - les relations sont chargées automatiquement quand on y accède (Lazy Loading avec proxies)
- Génère PLUSIEURS requêtes SQL (une pour les employés, puis une par employé pour charger les relations)

## Questions et Réponses

### Eager Loading

**Q: Pourquoi le département est vide?**
R: Sans `.Include(e => e.Departement)`, Entity Framework ne charge pas la relation par défaut. Il faut explicitement demander le chargement avec `Include()`.

**Q: Que faudrait-il changer dans le code pour qu'il y soit?**
R: Ajouter `.Include(e => e.Departement)` dans la requête LINQ (déjà fait dans le code final).

**Q: Quel est l'impact de ce changement?**
R: 
- Une seule requête SQL qui fait un JOIN pour récupérer toutes les données en une fois
- Plus performant quand on sait qu'on aura besoin des données
- Moins de requêtes à la base de données

**Q: Combien de temps prend la requête?**
R: À vérifier avec F12 > Réseau. Généralement:
- Première requête: Plus lente (compilation, connexion DB, etc.)
- Requêtes suivantes: Beaucoup plus rapide (cache, connexions réutilisées)

### Lazy Loading

**Configuration nécessaire:**
1. ✅ Package NuGet: `Microsoft.EntityFrameworkCore.Proxies`
2. ✅ Dans `Program.cs`: Ajouter `.UseLazyLoadingProxies()` au DbContext
3. ✅ Dans `Employe.cs`: Ajouter `virtual` aux propriétés de navigation:
   - `public virtual Pays? PaysOrigine { get; set; }`
   - `public virtual Departement? Departement { get; set; }`
   - `public virtual ICollection<Projet> ProjetsImpliques { get; set; }`

**Q: Que remarquez-vous avec le débogueur avant et après le passage de l'instruction `System.Diagnostics.Debug.WriteLine(e.Departement?.Nom)`?**
R: 
- **AVANT** l'accès à `e.Departement`: La valeur est NULL ou un proxy non chargé
- **APRÈS** l'accès: Entity Framework exécute automatiquement une requête SQL pour charger le département
- Visible dans la fenêtre de sortie: Une requête SQL pour CHAQUE employé!

**Q: Combien de temps prend la requête?**
R: À vérifier avec F12 > Réseau. Généralement:
- **BEAUCOUP plus lent** que Eager Loading!
- Génère N+1 requêtes (1 pour les employés + 1 par employé pour le département)
- Pour 100 employés: ~201 requêtes SQL (100 pour PaysOrigine, 100 pour Departement, 1 pour les employés)

## Résumé des différences

| Aspect | Eager Loading | Lazy Loading |
|--------|---------------|--------------|
| Nombre de requêtes | 1 requête SQL | N+1 requêtes SQL |
| Performance | ⚡ Rapide | 🐌 Lent |
| Utilisation mémoire | Plus élevée | Plus faible |
| Quand l'utiliser | Quand on sait qu'on aura besoin des données | Quand on ne sait pas si on aura besoin des données |
| Configuration | Juste `Include()` | Proxies + virtual + configuration |

## Recommandation

**Utilisez Eager Loading** dans la plupart des cas pour de meilleures performances!
