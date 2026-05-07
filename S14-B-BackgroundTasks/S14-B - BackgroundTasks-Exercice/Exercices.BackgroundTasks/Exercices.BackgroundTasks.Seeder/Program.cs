using Bogus;
using Exercices.BackgroundTasks.Seeder;
using Exercices.BackgroundTasks.Web.Models;

Console.WriteLine("Début du seed!");
using var context = DbContextFactory.CreateDbContext();

context.RemoveRange(context.Users);
context.SaveChanges();

// Création des Départements
var userFaker = new Faker<User>()
//.RuleFor(x => x.Id, 0);
.RuleFor(x => x.FirstName, f => f.Person.FirstName)
.RuleFor(x => x.LastName, f => f.Person.LastName)
.RuleFor(x => x.BirthDate, f => f.Person.DateOfBirth);

var users = userFaker.Generate(500);

// Ajout dans le ChangeTracker de EFCore
context.Users.AddRange(users);

// Enregistrement dans la BD
Console.WriteLine($"Sauvegarde des entités {nameof(User)} dans la BD");
context.SaveChanges();


