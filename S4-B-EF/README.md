# Exercice EF Core - Réponses aux questions

## Résumé de l'exercice

Cet exercice a permis de créer et configurer un projet ASP.NET Core MVC avec Entity Framework Core pour gérer des employés et leurs pays d'origine.

## Migrations créées

1. **InitialMigration** - Migration initiale qui crée les tables Employes et Pays
2. **AjoutSuperficiePays** - Ajout de la colonne SuperficieM2 à la table Pays
3. **AjoutProprieteNavigationPays** - Ajout d'une propriété de navigation (aucun changement en BD)
4. **RenommerPaysEnPaysOrigine** - Renommage de la propriété de navigation (aucun changement en BD)

## Réponses aux questions (Étape 14)

### Question 1: Regarder le contenu de la migration RenommerPaysEnPaysOrigine

Le contenu de la migration est **vide** - les méthodes `Up()` et `Down()` ne contiennent aucune instruction.

### Question 2: Pourquoi avez-vous ce contenu?

La migration est vide car **le renommage d'une propriété de navigation n'affecte pas la structure de la base de données**. 

Les propriétés de navigation sont des propriétés C# qui facilitent la navigation entre les entités dans le code, mais elles ne correspondent pas directement à des colonnes dans la base de données. 

Dans ce cas:
- La propriété `PaysId` (clé étrangère) existe toujours et n'a pas changé
- Seul le nom de la propriété de navigation C# a changé de `Pays` à `PaysOrigine`
- Cette modification est uniquement au niveau du modèle objet C# et n'a aucun impact sur le schéma de la base de données

### Question 3: Quel est le nom de la colonne dans votre base de données qui contient le pays d'origine d'un employé?

Le nom de la colonne dans la base de données est **`PaysId`**.

Cette colonne:
- Est une clé étrangère vers la table `Pays`
- A été créée lors de la migration initiale
- N'a jamais changé même si la propriété de navigation a été renommée
- Correspond à la propriété `public int PaysId { get; set; }` de la classe `Employe`

### Observations importantes

1. **Convention de nommage EF Core**: Par défaut, EF Core utilise le nom de la propriété de clé étrangère (ici `PaysId`) comme nom de colonne dans la base de données.

2. **Propriétés de navigation vs clés étrangères**:
   - `PaysId` est une propriété de clé étrangère → crée une colonne dans la BD
   - `PaysOrigine` est une propriété de navigation → ne crée pas de colonne, facilite seulement la navigation dans le code

3. **Relation entre les entités**: Le ModelSnapshot montre clairement la relation:
   ```csharp
   b.HasOne("S4_B_EF.Models.Pays", "PaysOrigine")
       .WithMany("Employes")
       .HasForeignKey("PaysId")
       .OnDelete(DeleteBehavior.Cascade)
       .IsRequired();
   ```
   
   Cela signifie:
   - Un Employe a un (HasOne) PaysOrigine
   - Un Pays a plusieurs (WithMany) Employes
   - La relation utilise la clé étrangère (HasForeignKey) "PaysId"
   - Si un Pays est supprimé, ses Employes sont également supprimés (Cascade)

## Structure de la base de données finale

### Table Employes
- `Id` (int, clé primaire, identity)
- `Nom` (nvarchar(max))
- `Age` (int)
- `DateEmbauche` (datetime2)
- `SalaireAnnuel` (float)
- `PaysId` (int, clé étrangère vers Pays)

### Table Pays
- `Id` (int, clé primaire, identity)
- `Nom` (nvarchar(max))
- `SuperficieM2` (int)

### Contraintes
- Clé étrangère: `FK_Employes_Pays_PaysId` avec suppression en cascade
- Index: `IX_Employes_PaysId` pour optimiser les requêtes
