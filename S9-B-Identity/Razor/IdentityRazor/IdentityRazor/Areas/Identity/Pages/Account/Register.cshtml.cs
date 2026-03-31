// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using IdentityRazor.Data;
using IdentityRazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRazor.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> TypesUtilisateur { get; } =
        [
            new SelectListItem("Employé", "Employe"),
            new SelectListItem("Étudiant", "Etudiant"),
            new SelectListItem("Enseignant", "Enseignant")
        ];

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Le {0} doit comporter au moins {2} et au plus {1} caractères.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Surnom")]
            public string Surnom { get; set; }

            [Required]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Display(Name = "Adresse")]
            public string Adresse { get; set; }

            [Required]
            [Display(Name = "Type d'utilisateur")]
            public string TypeUtilisateur { get; set; }

            // Employé
            [Display(Name = "Poste")]
            public string Poste { get; set; }

            [Display(Name = "Département")]
            public string Departement { get; set; }

            [Display(Name = "Date d'embauche")]
            [DataType(DataType.Date)]
            public DateTime? DateEmbauche { get; set; }

            // Étudiant
            [Display(Name = "Programme")]
            public string Programme { get; set; }

            [Display(Name = "Numéro étudiant")]
            public int? NumeroEtudiant { get; set; }

            [Display(Name = "Date d'inscription")]
            [DataType(DataType.Date)]
            public DateTime? DateInscription { get; set; }

            [Display(Name = "Moyenne générale")]
            public double? MoyenneGenerale { get; set; }

            // Enseignant
            [Display(Name = "Matière enseignée")]
            public string MatiereEnseignee { get; set; }

            [Display(Name = "Salaire")]
            public decimal? Salaire { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.Surnom = Input.Surnom;
                user.Nom = Input.Nom;
                user.Adresse = Input.Adresse;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Surnom", Input.Surnom));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("TypeUtilisateur", Input.TypeUtilisateur));

                    switch (Input.TypeUtilisateur)
                    {
                        case "Employe":
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Departement", Input.Departement ?? ""));
                            _context.Employes.Add(new Employe
                            {
                                UserId = user.Id,
                                Poste = Input.Poste ?? "",
                                Departement = Input.Departement!,
                                DateEmbauche = Input.DateEmbauche ?? DateTime.Today
                            });
                            await _userManager.AddClaimAsync(user, new Claim("Departement", Input.Departement!));
                            break;
                        case "Etudiant":
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Programme", Input.Programme ?? ""));
                            _context.Etudiants.Add(new Etudiant
                            {
                                UserId = user.Id,
                                Programme = Input.Programme ?? "",
                                NumeroEtudiant = Input.NumeroEtudiant ?? 0,
                                DateInscription = Input.DateInscription ?? DateTime.Today,
                                MoyenneGenerale = Input.MoyenneGenerale ?? 0.0
                            });
                            await _userManager.AddClaimAsync(user, new Claim("Programme", Input.Programme!));
                            break;
                        case "Enseignant":
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Matiere", Input.MatiereEnseignee ?? ""));
                            _context.Enseignants.Add(new Enseignant
                            {
                                UserId = user.Id,
                                MatiereEnseignee = Input.MatiereEnseignee ?? "",
                                DateEmbauche = Input.DateEmbauche ?? DateTime.Today,
                                Salaire = Input.Salaire ?? 0
                            });
                            await _userManager.AddClaimAsync(user, new Claim("Matiere", Input.MatiereEnseignee!));
                            break;
                    }
                    await _context.SaveChangesAsync();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
