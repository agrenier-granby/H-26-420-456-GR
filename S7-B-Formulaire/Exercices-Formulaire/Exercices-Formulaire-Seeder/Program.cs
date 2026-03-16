using Bogus;
using Exercice_Formulaire_Seeder;
using Exercices_Formulaire_Web.Models;

// https://github.com/bchavez/Bogus/tree/master/Examples/EFCoreSeedDb

Console.WriteLine("Début du seed!");
using var context = DbContextFactory.CreateDbContext();

// Vidange des tables
context.Employes.RemoveRange(context.Employes);
context.Departements.RemoveRange(context.Departements);
context.SaveChanges();

// Création des départements
var budgetFaker = new Faker();
List<Departement> departements =
[
    new() { Nom = "Finance", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
    new() { Nom = "IT", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
    new() { Nom = "RH", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
    new() { Nom = "Dev", Budget = float.Round(budgetFaker.Random.Float(20000, 65000), 2) },
];

context.Departements.AddRange(departements);
context.SaveChanges();

List<Pays> pays = [.. context.Pays];

// Création des employés conformes aux validations
var faker = new Faker("fr_CA");
var employes = new List<Employe>();
var gestionnaireParDepartement = new HashSet<int>();

for (var i = 0; i < 500; i++)
{
    var departement = faker.PickRandom(departements);
    var paysChoisi = faker.PickRandom(pays);

    var titre = faker.PickRandom<Titre>();
    if (titre == Titre.GestionnaireDeProjet && gestionnaireParDepartement.Contains(departement.Id))
    {
        titre = faker.PickRandom(new[] { Titre.ChefDEquipe, Titre.Developpeur, Titre.Testeur });
    }

    if (titre == Titre.GestionnaireDeProjet)
    {
        gestionnaireParDepartement.Add(departement.Id);
    }

    var salaireMinimum = titre switch
    {
        Titre.Testeur => 40000,
        Titre.Developpeur => 60000,
        Titre.ChefDEquipe => 80000,
        Titre.GestionnaireDeProjet => 100000,
        _ => 40000
    };

    var dateEmbauche = faker.Date.Between(new DateTime(2017, 1, 1), new DateTime(2022, 2, 2)).Date;
    var dateNaissanceMax = dateEmbauche.AddYears(-18);
    var dateNaissanceMin = dateEmbauche.AddYears(-75);
    var dateNaissance = faker.Date.Between(dateNaissanceMin, dateNaissanceMax).Date;

    var age = DateTime.Today.Year - dateNaissance.Year;
    if (dateNaissance.Date > DateTime.Today.AddYears(-age))
    {
        age--;
    }

    var statut = faker.PickRandom<StatutEmploye>();
    DateTime? dateDepart = null;
    if (statut == StatutEmploye.Inactif)
    {
        dateDepart = faker.Date.Between(dateEmbauche.AddMonths(1), DateTime.Today).Date;
    }

    employes.Add(new Employe
    {
        Nom = faker.Person.FullName,
        Age = Math.Clamp(age, 18, 75),
        DateNaissance = dateNaissance,
        DateEmbauche = dateEmbauche,
        DateDepart = dateDepart,
        SalaireAnnuel = float.Round(faker.Random.Float(salaireMinimum, 120000), 2),
        Titre = titre,
        Statut = statut,
        TypeRemuneration = faker.PickRandom<TypeRemuneration>(),
        PaysId = paysChoisi.Id,
        DepartementId = departement.Id
    });
}

context.Employes.AddRange(employes);
Console.WriteLine($"Sauvegarde des entités {nameof(Employe)} dans la BD");
context.SaveChanges();
