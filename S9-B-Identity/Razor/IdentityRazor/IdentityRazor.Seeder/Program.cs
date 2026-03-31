using IdentityRazor.Models;
using IdentityMVC.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Debut du seed!");

using var context = DbContextFactory.CreateDbContext();

// --- Vidange des utilisateurs seed existants ---
Console.WriteLine("Nettoyage des donnees existantes...");

var seedEmails = new[] { "employe@test.com", "etudiant@test.com", "enseignant@test.com" };
var existingUsers = await context.Users.Where(u => seedEmails.Contains(u.Email!)).ToListAsync();
var existingIds = existingUsers.Select(u => u.Id).ToList();

context.Employes.RemoveRange(context.Employes.Where(e => existingIds.Contains(e.UserId)));
context.Etudiants.RemoveRange(context.Etudiants.Where(e => existingIds.Contains(e.UserId)));
context.Enseignants.RemoveRange(context.Enseignants.Where(e => existingIds.Contains(e.UserId)));
context.UserClaims.RemoveRange(context.UserClaims.Where(c => existingIds.Contains(c.UserId)));
context.Users.RemoveRange(existingUsers);
await context.SaveChangesAsync();

var hasher = new PasswordHasher<ApplicationUser>();

// --- Employe ---
Console.WriteLine("Creation de l'employe...");
var employe = new ApplicationUser
{
    Id = Guid.NewGuid().ToString(),
    UserName = "employe@test.com",
    NormalizedUserName = "employe@test.com".ToUpper(),
    Email = "employe@test.com",
    NormalizedEmail = "employe@test.com".ToUpper(),
    EmailConfirmed = true,
    Surnom = "Bob",
    Nom = "Tremblay",
    SecurityStamp = Guid.NewGuid().ToString(),
    ConcurrencyStamp = Guid.NewGuid().ToString()
};
employe.PasswordHash = hasher.HashPassword(employe, "Test1234!");
context.Users.Add(employe);
context.UserClaims.AddRange(
    new IdentityUserClaim<string> { UserId = employe.Id, ClaimType = "Surnom",          ClaimValue = employe.Surnom },
    new IdentityUserClaim<string> { UserId = employe.Id, ClaimType = "TypeUtilisateur",  ClaimValue = "Employe" },
    new IdentityUserClaim<string> { UserId = employe.Id, ClaimType = "Departement",      ClaimValue = "Informatique" }
);
context.Employes.Add(new Employe
{
    UserId = employe.Id,
    Poste = "Developpeur",
    Departement = "Informatique",
    DateEmbauche = new DateTime(2020, 1, 15)
});

// --- Etudiant ---
Console.WriteLine("Creation de l'etudiant...");
var etudiant = new ApplicationUser
{
    Id = Guid.NewGuid().ToString(),
    UserName = "etudiant@test.com",
    NormalizedUserName = "etudiant@test.com".ToUpper(),
    Email = "etudiant@test.com",
    NormalizedEmail = "etudiant@test.com".ToUpper(),
    EmailConfirmed = true,
    Surnom = "Alice",
    Nom = "Gagnon",
    SecurityStamp = Guid.NewGuid().ToString(),
    ConcurrencyStamp = Guid.NewGuid().ToString()
};
etudiant.PasswordHash = hasher.HashPassword(etudiant, "Test1234!");
context.Users.Add(etudiant);
context.UserClaims.AddRange(
    new IdentityUserClaim<string> { UserId = etudiant.Id, ClaimType = "Surnom",          ClaimValue = etudiant.Surnom },
    new IdentityUserClaim<string> { UserId = etudiant.Id, ClaimType = "TypeUtilisateur",  ClaimValue = "Etudiant" },
    new IdentityUserClaim<string> { UserId = etudiant.Id, ClaimType = "Programme",        ClaimValue = "Techniques de l'informatique" }
);
context.Etudiants.Add(new Etudiant
{
    UserId = etudiant.Id,
    Programme = "Techniques de l'informatique",
    NumeroEtudiant = 12345,
    DateInscription = new DateTime(2024, 8, 28),
    MoyenneGenerale = 3.8
});

// --- Enseignant ---
Console.WriteLine("Creation de l'enseignant...");
var enseignant = new ApplicationUser
{
    Id = Guid.NewGuid().ToString(),
    UserName = "enseignant@test.com",
    NormalizedUserName = "enseignant@test.com".ToUpper(),
    Email = "enseignant@test.com",
    NormalizedEmail = "enseignant@test.com".ToUpper(),
    EmailConfirmed = true,
    Surnom = "Charles",
    Nom = "Dupont",
    SecurityStamp = Guid.NewGuid().ToString(),
    ConcurrencyStamp = Guid.NewGuid().ToString()
};
enseignant.PasswordHash = hasher.HashPassword(enseignant, "Test1234!");
context.Users.Add(enseignant);
context.UserClaims.AddRange(
    new IdentityUserClaim<string> { UserId = enseignant.Id, ClaimType = "Surnom",          ClaimValue = enseignant.Surnom },
    new IdentityUserClaim<string> { UserId = enseignant.Id, ClaimType = "TypeUtilisateur",  ClaimValue = "Enseignant" },
    new IdentityUserClaim<string> { UserId = enseignant.Id, ClaimType = "Matiere",           ClaimValue = "Programmation Web II" }
);
context.Enseignants.Add(new Enseignant
{
    UserId = enseignant.Id,
    MatiereEnseignee = "Programmation Web II",
    DateEmbauche = new DateTime(2018, 9, 1),
    Salaire = 75000
});

await context.SaveChangesAsync();
Console.WriteLine("Seed termine avec succes!");
Console.WriteLine($"  employe@test.com    -> Employe   (Bob Tremblay)");
Console.WriteLine($"  etudiant@test.com   -> Etudiant  (Alice Gagnon)");
Console.WriteLine($"  enseignant@test.com -> Enseignant (Charles Dupont)");
Console.WriteLine("Mot de passe de tous : Test1234!");
