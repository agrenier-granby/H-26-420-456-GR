using Bogus;
using Exercice_Formulaire_Seeder;
using Exercices_Formulaire_Web.Models;

// https://github.com/bchavez/Bogus/tree/master/Examples/EFCoreSeedDb

Console.WriteLine("Début du seed!");
using var context = DbContextFactory.CreateDbContext();

// Vidange de la table
context.Departements.RemoveRange(context.Departements);
context.Employes.RemoveRange(context.Employes);
context.SaveChanges();

//Création des départements
var budgetFaker = new Faker();
List<Departement> departements =
            [
                new Departement() { Nom = "Finance", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
                new Departement() { Nom = "IT", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
                new Departement() { Nom = "RH", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
                new Departement() { Nom = "Dev", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
            ];

context.Departements.AddRange(departements);
context.SaveChanges();

List<Pays> pays = [.. context.Pays];

//Création des employés
var generator = new Faker();

var employesFaker = new Faker<Employe>()
    .RuleFor(e => e.Id, 0) // Nécessaire à cause de IDENTITY dans la BD
    .RuleFor(e => e.Nom, f => f.Person.FullName)
    .RuleFor(e => e.Age, f => f.Random.Int(22, 55))
    .RuleFor(e => e.DateEmbauche, f => f.Date.Between(new DateTime(2017, 01, 01), new DateTime(2022, 02, 02)))
    .RuleFor(e => e.SalaireAnnuel, f => f.Random.Float(38000, 95000))
    .RuleFor(e => e.PaysOrigine, f => f.PickRandom(pays))
    .RuleFor(e => e.DepartementId, f => f.PickRandom(departements).Id)
    .RuleFor(e => e.Statut, f => f.PickRandom<StatutEmploye>())
    .RuleFor(e => e.TypeRemuneration, f => f.PickRandom<TypeRemuneration>());

var employes = employesFaker.Generate(500);

context.Employes.AddRange(employes);
Console.WriteLine($"Sauvegarde des entités {nameof(Employe)} dans la BD");
context.SaveChanges();
