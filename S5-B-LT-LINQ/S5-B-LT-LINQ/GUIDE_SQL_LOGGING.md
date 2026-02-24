# Guide pour voir les requêtes SQL générées

## Configuration

Le projet a été configuré dans `Program.cs` pour afficher toutes les requêtes SQL:

```csharp
.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
.EnableSensitiveDataLogging()
.EnableDetailedErrors()
```

## Comment voir les requêtes SQL?

### Option 1: Console de sortie Visual Studio (Recommandé)
1. Dans Visual Studio, allez dans **Affichage > Sortie** (ou `Ctrl+Alt+O`)
2. Dans le menu déroulant en haut, sélectionnez **"Debug"** ou **"Serveur web ASP.NET Core"**
3. Lancez l'application en mode Debug (F5)
4. Naviguez vers `/Home/EagerLoading` ou `/Home/LazyLoading`
5. **Observez les requêtes SQL dans la fenêtre de sortie!**

### Option 2: Terminal/Console
Si vous lancez avec `dotnet run`, les requêtes SQL s'afficheront directement dans le terminal.

### Option 3: Fenêtre de débogage
Les requêtes du `Debug.WriteLine()` dans LazyLoading apparaîtront dans la fenêtre de sortie Debug.

---

## Exemple de sortie - Eager Loading

### Action: `/Home/EagerLoading`

```sql
-- UNE SEULE requête avec JOINs
SELECT TOP(100) 
    [e].[Id], [e].[Age], [e].[DateEmbauche], [e].[DepartementId], 
    [e].[Nom], [e].[PaysId], [e].[SalaireAnnuel],
    [p].[Id], [p].[Nom], [p].[SuperficieM2],
    [d].[Id], [d].[BudgetAnnuel], [d].[DirecteurId], [d].[Nom]
FROM [Employes] AS [e]
LEFT JOIN [Pays] AS [p] ON [e].[PaysId] = [p].[Id]
LEFT JOIN [Departements] AS [d] ON [e].[DepartementId] = [d].[Id]
```

**Résultat:** 
- ✅ **1 requête SQL**
- ✅ Performance optimale
- ✅ Un seul aller-retour vers la base de données
- ✅ Temps de réponse: ~50-200ms

---

## Exemple de sortie - Lazy Loading

### Action: `/Home/LazyLoading`

```sql
-- 1ère requête : Les employés seulement
SELECT TOP(100) 
    [e].[Id], [e].[Age], [e].[DateEmbauche], [e].[DepartementId], 
    [e].[Nom], [e].[PaysId], [e].[SalaireAnnuel]
FROM [Employes] AS [e]

-- 2ème requête : Le département du 1er employé (dans le foreach)
SELECT [d].[Id], [d].[BudgetAnnuel], [d].[DirecteurId], [d].[Nom]
FROM [Departements] AS [d]
WHERE [d].[Id] = @__p_0

-- 3ème requête : Le pays du 1er employé (dans la vue)
SELECT [p].[Id], [p].[Nom], [p].[SuperficieM2]
FROM [Pays] AS [p]
WHERE [p].[Id] = @__p_0

-- 4ème requête : Le département du 2e employé
SELECT [d].[Id], [d].[BudgetAnnuel], [d].[DirecteurId], [d].[Nom]
FROM [Departements] AS [d]
WHERE [d].[Id] = @__p_0

-- 5ème requête : Le pays du 2e employé
SELECT [p].[Id], [p].[Nom], [p].[SuperficieM2]
FROM [Pays] AS [p]
WHERE [p].[Id] = @__p_0

-- ... ET AINSI DE SUITE pour les 100 employés! ...

-- 200ème requête : Le département du 100e employé
-- 201ème requête : Le pays du 100e employé
```

**Résultat:**
- ❌ **201 requêtes SQL** (1 + 100 + 100)
- ❌ Performance médiocre
- ❌ 201 allers-retours vers la base de données
- ❌ Temps de réponse: ~2000-5000ms (10-25x plus lent!)

---

## Le problème N+1

Le **problème N+1** est un anti-pattern classique:
- **1 requête** pour obtenir N entités
- **N requêtes** supplémentaires pour charger les relations
- **Total: N+1 requêtes**

### Avec 100 employés:
- Eager Loading: **1 requête** ✅
- Lazy Loading: **201 requêtes** ❌

### Avec 1000 employés:
- Eager Loading: **1 requête** ✅
- Lazy Loading: **2001 requêtes** ❌❌❌

---

## Comparaison visuelle

### Eager Loading (Optimal)
```
[Client] ----1 requête----> [Base de données]
         <---Toutes les données---
```

### Lazy Loading (Problème N+1)
```
[Client] ----1 requête (employés)----> [Base de données]
         <---100 employés---
         
         ----Requête pays #1----> [Base de données]
         <---Pays #1---
         
         ----Requête dept #1----> [Base de données]
         <---Dept #1---
         
         ----Requête pays #2----> [Base de données]
         <---Pays #2---
         
         ... (198 autres requêtes!) ...
```

---

## Recommandations

### ✅ Utilisez Eager Loading quand:
- Vous savez que vous aurez besoin des données liées
- Vous affichez les données dans une vue/liste
- La performance est importante

### ⚠️ Utilisez Lazy Loading quand:
- Vous ne savez PAS si vous aurez besoin des données
- Vous chargez une seule entité (pas une liste)
- Les données liées sont rarement utilisées

### ❌ N'utilisez JAMAIS Lazy Loading pour:
- Charger des listes/collections
- Afficher des données dans une vue
- Des boucles qui accèdent aux propriétés de navigation

---

## Exercice pratique

1. **Lancez l'application en mode Debug**
2. **Ouvrez la fenêtre Sortie** (Ctrl+Alt+O)
3. **Testez `/Home/EagerLoading`**
   - Comptez le nombre de requêtes SQL
   - Notez le temps de réponse (F12 > Réseau)
4. **Testez `/Home/LazyLoading`**
   - Comptez le nombre de requêtes SQL
   - Notez le temps de réponse (F12 > Réseau)
5. **Comparez les résultats!**

### Questions de réflexion:
- Quelle approche est la plus rapide?
- Pourquoi y a-t-il autant de requêtes avec Lazy Loading?
- Dans quels cas Lazy Loading pourrait-il être utile?
- Comment éviter le problème N+1?
