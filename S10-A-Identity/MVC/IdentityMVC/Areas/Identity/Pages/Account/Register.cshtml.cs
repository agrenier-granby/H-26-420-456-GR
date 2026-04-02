using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IdentityMVC.Data;
using IdentityMVC.Models;

namespace IdentityMVC.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<RegisterModel> logger,
    IUserStore<ApplicationUser> userStore,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext dbContext) : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<RegisterModel> _logger = logger;
    private readonly IUserStore<ApplicationUser> _userStore = userStore;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _dbContext = dbContext;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = [];

    public List<SelectListItem> TypesUtilisateur { get; } =
    [
        new SelectListItem("Employé", "Employe"),
        new SelectListItem("Étudiant", "Etudiant"),
        new SelectListItem("Enseignant", "Enseignant")
    ];

    public class InputModel
    {
        // --- Partie 1 : champs de base ---
        [Required]
        [Display(Name = "Surnom")]
        public string Surnom { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Adresse")]
        public string? Adresse { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Le champ {0} doit contenir entre {2} et {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // --- Type d'utilisateur ---
        [Required(ErrorMessage = "Veuillez selectionner un type d'utilisateur.")]
        [Display(Name = "Type d'utilisateur")]
        public string TypeUtilisateur { get; set; } = string.Empty;

        // --- Employe ---
        [Display(Name = "Poste")]
        public string? Poste { get; set; }

        [Display(Name = "Departement")]
        public string? Departement { get; set; }

        [Display(Name = "Date d'embauche")]
        public DateTime? DateEmbaucheEmploye { get; set; }

        // --- Etudiant ---
        [Display(Name = "Programme")]
        public string? Programme { get; set; }

        [Display(Name = "Numero etudiant")]
        public int? NumeroEtudiant { get; set; }

        [Display(Name = "Date d'inscription")]
        public DateTime? DateInscription { get; set; }

        [Display(Name = "Moyenne generale")]
        public double? MoyenneGenerale { get; set; }

        // --- Enseignant ---
        [Display(Name = "Matiere enseignee")]
        public string? MatiereEnseignee { get; set; }

        [Display(Name = "Date d'embauche")]
        public DateTime? DateEmbaucheEnseignant { get; set; }

        [Display(Name = "Salaire")]
        public double? Salaire { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ValidateTypeFields();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = CreateUser();
        user.Email = Input.Email;
        user.UserName = Input.Email;
        user.Nom = Input.Nom;
        user.Surnom = Input.Surnom;
        user.Adresse = Input.Adresse;

        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");
            await _userManager.AddClaimAsync(user, new Claim("Surnom", Input.Surnom));
            await _userManager.AddClaimAsync(user, new Claim("TypeUtilisateur", Input.TypeUtilisateur));
            await CreateSpecializedUserAsync(user);
            await AssignDefaultRoleAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }

    private void ValidateTypeFields()
    {
        switch (Input.TypeUtilisateur)
        {
            case "Employe":
                if (string.IsNullOrWhiteSpace(Input.Poste))
                    ModelState.AddModelError("Input.Poste", "Le poste est obligatoire pour un employe.");
                if (string.IsNullOrWhiteSpace(Input.Departement))
                    ModelState.AddModelError("Input.Departement", "Le departement est obligatoire pour un employe.");
                break;
            case "Etudiant":
                if (string.IsNullOrWhiteSpace(Input.Programme))
                    ModelState.AddModelError("Input.Programme", "Le programme est obligatoire pour un etudiant.");
                break;
            case "Enseignant":
                if (string.IsNullOrWhiteSpace(Input.MatiereEnseignee))
                    ModelState.AddModelError("Input.MatiereEnseignee", "La matiere enseignee est obligatoire pour un enseignant.");
                break;
        }
    }

    private async Task CreateSpecializedUserAsync(ApplicationUser user)
    {
        switch (Input.TypeUtilisateur)
        {
            case "Employe":
                _dbContext.Employes.Add(new Employe
                {
                    UserId = user.Id,
                    Poste = Input.Poste!,
                    Departement = Input.Departement!,
                    DateEmbauche = Input.DateEmbaucheEmploye ?? DateTime.Today
                });
                await _userManager.AddClaimAsync(user, new Claim("Departement", Input.Departement!));
                break;
            case "Etudiant":
                _dbContext.Etudiants.Add(new Etudiant
                {
                    UserId = user.Id,
                    Programme = Input.Programme!,
                    NumeroEtudiant = Input.NumeroEtudiant ?? 0,
                    DateInscription = Input.DateInscription ?? DateTime.Today,
                    MoyenneGenerale = Input.MoyenneGenerale ?? 0
                });
                await _userManager.AddClaimAsync(user, new Claim("Programme", Input.Programme!));
                break;
            case "Enseignant":
                _dbContext.Enseignants.Add(new Enseignant
                {
                    UserId = user.Id,
                    MatiereEnseignee = Input.MatiereEnseignee!,
                    DateEmbauche = Input.DateEmbaucheEnseignant ?? DateTime.Today,
                    Salaire = Input.Salaire ?? 0
                });
                await _userManager.AddClaimAsync(user, new Claim("Matiere", Input.MatiereEnseignee!));
                break;
        }
        await _dbContext.SaveChangesAsync();
    }

    private async Task AssignDefaultRoleAsync(ApplicationUser user)
    {
        if (!await _roleManager.RoleExistsAsync(Roles.Membre))
            await _roleManager.CreateAsync(new IdentityRole(Roles.Membre));

        await _userManager.AddToRoleAsync(user, Roles.Membre);
    }

    private static ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. Ensure it has a parameterless constructor and is not abstract.");
        }
    }
}
