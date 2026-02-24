# Instructions rapides - Voir les requêtes SQL

## Pour les étudiants

### Étapes simples:

1. **Ouvrir la fenêtre de sortie dans Visual Studio**
   - Menu: `Affichage` > `Sortie`
   - Ou appuyez sur: `Ctrl+Alt+O`

2. **Sélectionner la bonne sortie**
   - Dans le menu déroulant en haut de la fenêtre Sortie
   - Sélectionnez: **"Serveur web ASP.NET Core"** ou **"Debug"**

3. **Lancer l'application**
   - Appuyez sur `F5` (mode Debug)

4. **Tester Eager Loading**
   - Allez à: `https://localhost:XXXX/Home/EagerLoading`
   - Regardez la fenêtre Sortie
   - **Vous devriez voir 1 SEULE requête SQL avec JOIN**

5. **Tester Lazy Loading**
   - Allez à: `https://localhost:XXXX/Home/LazyLoading`
   - Regardez la fenêtre Sortie
   - **Vous devriez voir BEAUCOUP de requêtes SQL (200+)**

---

## Ce que vous devriez observer:

### ✅ Eager Loading - Exemple de sortie
```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (50ms) [Parameters=[]]
      SELECT TOP(100) [e].[Id], [e].[Nom], [e].[Age], [p].[Nom], [d].[Nom]
      FROM [Employes] AS [e]
      LEFT JOIN [Pays] AS [p] ON [e].[PaysId] = [p].[Id]
      LEFT JOIN [Departements] AS [d] ON [e].[DepartementId] = [d].[Id]
```
**➜ UNE SEULE REQUÊTE!**

---

### ❌ Lazy Loading - Exemple de sortie
```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (20ms) [Parameters=[]]
      SELECT TOP(100) [e].[Id], [e].[Nom], [e].[Age]
      FROM [Employes] AS [e]

info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[@__p_0='1']]
      SELECT [p].[Id], [p].[Nom]
      FROM [Pays] AS [p]
      WHERE [p].[Id] = @__p_0

info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[@__p_0='1']]
      SELECT [d].[Id], [d].[Nom]
      FROM [Departements] AS [d]
      WHERE [d].[Id] = @__p_0

info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[@__p_0='2']]
      SELECT [p].[Id], [p].[Nom]
      FROM [Pays] AS [p]
      WHERE [p].[Id] = @__p_0

... (197 autres requêtes!) ...
```
**➜ 201 REQUÊTES! (Problème N+1)**

---

## Timing avec F12

Ouvrez aussi les outils développeur (F12) et allez dans l'onglet **Réseau**:

- **Eager Loading**: ~50-200ms ⚡
- **Lazy Loading**: ~2000-5000ms 🐌 (10-25x plus lent!)

---

## Conclusion

Le **Lazy Loading** génère le fameux problème **N+1**:
- 1 requête pour les employés
- N requêtes pour les pays (1 par employé)
- N requêtes pour les départements (1 par employé)
- **Total: 1 + N + N = 201 requêtes pour 100 employés!**

Le **Eager Loading** résout ce problème avec `Include()`:
- 1 seule requête avec JOINs
- **Total: 1 requête pour 100 employés! ✅**

---

**Règle d'or**: Utilisez toujours `Include()` quand vous savez que vous aurez besoin des données liées!
