using Bogus;
using Exercices.Middlewares.Models;
using Exercices.Middlewares.Seeder;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Début du seed!");

using var context = DbContextFactory.CreateDbContext();
// Vidange de la table
context.Departements.RemoveRange(context.Departements);
context.Employes.RemoveRange(context.Employes);
context.SaveChanges();

//Création des départements
List<Departement> listeDepartement =
            [
                new Departement() { Nom = "Finance", Budget = Random.Shared.Next(20000, 65000) },
                new Departement() { Nom = "Informatique", Budget = Random.Shared.Next(20000, 65000) },
                new Departement() { Nom = "Ressources humaines", Budget = Random.Shared.Next(20000, 65000) },
                new Departement() { Nom = "Développement", Budget = Random.Shared.Next(20000, 65000) },
            ];

context.Departements.AddRange(listeDepartement);
context.SaveChanges();

List<Pays> listePays = [.. context.Pays];

var employesFaker = new Faker<Employe>();
employesFaker
        .RuleFor(e => e.Nom, f => f.Name.FullName())
        .RuleFor(e => e.Age, f => f.Random.Int(18, 75))
        .RuleFor(e => e.DateEmbauche, f => f.Date.Between(new DateTime(2017, 1, 1), new DateTime(2020, 1, 1)))
        .RuleFor(e => e.Salaire, f => f.Random.Int(38000, 95000))
        .RuleFor(e => e.EstEnEmploi, f => f.Random.Bool(0.95f))
        .RuleFor(e => e.PaysOrigine, f => f.PickRandom(listePays))
        .RuleFor(e => e.Departement, f => f.PickRandom(listeDepartement));

context.Employes.AddRange(employesFaker.Generate(500));
context.Employes.AsNoTracking(); // Pour éviter de retourner à la BD pour chaque entité ajoutée et récupérer leur ID auto-généré
context.SaveChanges();
Console.WriteLine("Fin du seed!");