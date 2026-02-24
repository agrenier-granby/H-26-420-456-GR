using Bogus;
using S5_A_SEEDER.Models;
using S5_A_SEEDER.Seeder;

public partial class Program
{
    private static void Main(string[] args)
    {
        using var context = DbContextFactory.CreateDbContext();


        // --- Faker Pays ---
        var paysFaker = new Faker<Pays>("fr")
            .RuleFor(x => x.Id, 0) // laisser EF auto-générer
            .RuleFor(x => x.Nom, f => f.Address.Country());

        // --- Faker Département ---
        var departementFaker = new Faker<Departement>("fr")
            .RuleFor(x => x.Id, 0)
            .RuleFor(x => x.Nom, f =>
            {
                var name = f.Commerce.Department();
                // respecter [MaxLength(50)] si présent
                return name.Length > 50 ? name.Substring(0, 50) : name;
            })
            .RuleFor(x => x.BudgetAnnuel, f => Math.Round(f.Random.Double(200_000, 5_000_000), 2))
            .RuleFor(x => x.DirecteurId, _ => null)  // sera fixé après création des employés
            .RuleFor(x => x.Directeur, _ => null);

        // --- Générer Pays et Départements d'abord ---
        var pays = paysFaker.Generate(10);
        var departements = departementFaker.Generate(6);

        context.AddRange(pays);
        context.AddRange(departements);
        context.SaveChanges();
    }
}