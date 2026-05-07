// See https://aka.ms/new-console-template for more information
using Bogus;
using EFCore.Seeder;
using Exercices.Courriels.Web.Models;

Console.WriteLine("Hello, World!");

var context = DbContextFactory.CreateDbContext();

context.Subscribers.RemoveRange();
context.SaveChanges();

var subscribersFaker = new Faker<Subscriber>();
subscribersFaker
    .RuleFor(s => s.Email, f => f.Internet.Email())
    .RuleFor(s => s.IsConfirmed, f => f.Random.Bool(0.8f));

var subcribers = subscribersFaker.Generate(5_000);

context.Subscribers.AddRange(subcribers);
context.SaveChanges();

Console.WriteLine("Terminé");