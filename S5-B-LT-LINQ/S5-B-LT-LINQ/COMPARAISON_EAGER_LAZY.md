# Tableau Comparatif - Eager vs Lazy Loading

## Résumé visuel pour les étudiants

| Caractéristique | Eager Loading | Lazy Loading |
|----------------|---------------|--------------|
| **Méthode** | `.Include()` | `virtual` + proxies |
| **Nombre de requêtes** | 1 | N+1 (201 pour 100 employés) |
| **Performance** | ⚡ Rapide (~50-200ms) | 🐌 Lent (~2000-5000ms) |
| **Requête SQL** | `SELECT ... JOIN ...` | `SELECT ... WHERE Id = ?` |
| **Chargement** | Immédiat | À la demande |
| **Utilisation mémoire** | Plus élevée | Plus faible |
| **Complexité** | Simple | Proxies requis |
| **Configuration** | Juste `Include()` | `virtual` + `UseLazyLoadingProxies()` |

---

## Comparaison des requêtes SQL

### Eager Loading - 1 requête
```sql
SELECT TOP(100) 
    [e].[Id], [e].[Nom], [e].[Age], [e].[SalaireAnnuel],
    [p].[Id], [p].[Nom],           -- Pays chargé ici
    [d].[Id], [d].[Nom]            -- Département chargé ici
FROM [Employes] AS [e]
LEFT JOIN [Pays] AS [p] ON [e].[PaysId] = [p].[Id]
LEFT JOIN [Departements] AS [d] ON [e].[DepartementId] = [d].[Id]
```

### Lazy Loading - 201 requêtes (exemple pour 2 employés)
```sql
-- Requête 1: Les employés
SELECT TOP(100) [e].[Id], [e].[Nom], [e].[Age]
FROM [Employes] AS [e]

-- Requête 2: Pays du 1er employé
SELECT [p].[Id], [p].[Nom]
FROM [Pays] AS [p]
WHERE [p].[Id] = 1

-- Requête 3: Département du 1er employé
SELECT [d].[Id], [d].[Nom]
FROM [Departements] AS [d]
WHERE [d].[Id] = 1

-- Requête 4: Pays du 2e employé
SELECT [p].[Id], [p].[Nom]
FROM [Pays] AS [p]
WHERE [p].[Id] = 2

-- Requête 5: Département du 2e employé
SELECT [d].[Id], [d].[Nom]
FROM [Departements] AS [d]
WHERE [d].[Id] = 1

-- ... 196 autres requêtes pour les 98 autres employés!
```

---

## Impact sur la performance

### Scénario: Afficher 100 employés avec leur pays et département

| Métrique | Eager Loading | Lazy Loading | Différence |
|----------|---------------|--------------|------------|
| Requêtes SQL | 1 | 201 | 200x plus! |
| Allers-retours DB | 1 | 201 | 200x plus! |
| Temps réseau | ~5ms | ~1000ms | 200x plus lent! |
| Temps total | ~50-200ms | ~2000-5000ms | 10-25x plus lent! |
| Utilisation réseau | 1 connexion | 201 connexions | 200x plus! |

---

## Quand utiliser chaque approche?

### ✅ Utilisez Eager Loading (Include) pour:
- **Afficher des listes** (ex: liste d'employés avec leurs infos)
- **Rapports** nécessitant plusieurs relations
- **Vues** affichant des propriétés de navigation
- **APIs** retournant des données complètes
- **Performance critique**

### ⚠️ Utilisez Lazy Loading pour:
- **Navigation conditionnelle** (accès rare aux relations)
- **Entités individuelles** (pas de liste)
- **Prototypage rapide** (pas pour la production!)
- **Relations optionnelles** rarement utilisées

### ❌ N'utilisez JAMAIS Lazy Loading pour:
- **Boucles** (`foreach`) accédant aux relations
- **Listes/Collections** avec relations
- **Vues Razor** affichant des propriétés de navigation
- **APIs publiques** (performances imprévisibles)

---

## Code comparatif

### ✅ Bon - Eager Loading
```csharp
public IActionResult ListeEmployes()
{
    var employes = _context.Employes
        .Include(e => e.PaysOrigine)    // 👍 Chargé en 1 requête
        .Include(e => e.Departement)    // 👍 Chargé en 1 requête
        .ToList();
    return View(employes);
}
// Résultat: 1 requête SQL
```

### ❌ Mauvais - Lazy Loading dans une liste
```csharp
public IActionResult ListeEmployes()
{
    var employes = _context.Employes
        .ToList();                      // ❌ Pas de Include
    return View(employes);
}
// Dans la vue: @e.PaysOrigine.Nom
// Résultat: N+1 requêtes SQL (1 + 100 + 100 = 201)
```

---

## Le problème N+1 illustré

### Avec 10 employés: 21 requêtes
```
Employés:       1 requête
Pays (10x):    10 requêtes
Départements:  10 requêtes
--------------------------
Total:         21 requêtes
```

### Avec 100 employés: 201 requêtes
```
Employés:        1 requête
Pays (100x):   100 requêtes
Départements:  100 requêtes
--------------------------
Total:         201 requêtes
```

### Avec 1000 employés: 2001 requêtes
```
Employés:         1 requête
Pays (1000x):  1000 requêtes
Départements:  1000 requêtes
--------------------------
Total:         2001 requêtes ⚠️⚠️⚠️
```

**Avec Eager Loading: TOUJOURS 1 requête! ✅**

---

## Conclusion

Le **Lazy Loading** est pratique pour le développement rapide, mais il peut causer des **problèmes de performance majeurs** en production.

**Règle d'or pour les étudiants:**
> Si vous affichez une liste ou utilisez une boucle, utilisez **TOUJOURS** `.Include()` pour charger les relations!

**Formule du problème N+1:**
```
Nombre de requêtes = 1 + (N × nombre de relations)
```

Avec 100 employés et 2 relations:
```
Lazy Loading:  1 + (100 × 2) = 201 requêtes ❌
Eager Loading: 1 requête ✅
```

---

## Exercice pratique

1. Lancez l'application
2. Testez `/Home/EagerLoading` et comptez les requêtes
3. Testez `/Home/LazyLoading` et comptez les requêtes
4. Mesurez les temps avec F12 > Réseau
5. Calculez la différence de performance

**Question bonus:** Si vous avez 500 employés, combien de requêtes Lazy Loading générera-t-il?
<details>
<summary>Réponse</summary>
1 + (500 × 2) = 1001 requêtes! 🤯
</details>
