using BackgroundTasks.Seeder.Exemple;
using BackgroundTasks.Web.Exemple.Models;
using Bogus;

Console.WriteLine("Début du seed!");
using var context = DbContextFactory.CreateDbContext();

context.RemoveRange(context.Users);
context.SaveChanges();

// Création des Départements
var userFaker = new Faker<User>()
//.RuleFor(x => x.Id, 0);
.RuleFor(x => x.FullName, (f, x) => f.Commerce.Department())
.RuleFor(x => x.Email, f => f.Person.Email)
.RuleFor(x => x.CreatedAt, f => DateTime.Now)
.RuleFor(x => x.DateOfBirth, f => DateOnly.FromDateTime(f.Person.DateOfBirth))
.RuleFor(x => x.LastSuccessfulLoginAt, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(-3)));

var users = userFaker.Generate(50);

// Ajout dans le ChangeTracker de EFCore
context.Users.AddRange(users);

// Enregistrement dans la BD
Console.WriteLine($"Sauvegarde des entités {nameof(User)} dans la BD");
context.SaveChanges();


