using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVC.Controllers
{
    public class RoleExempleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleExempleController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // ----------------------------------------------------------------------
        // 1. CRÉER UN RÔLE
        // ----------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return Content("Nom de rôle invalide.");

            if (await _roleManager.RoleExistsAsync(roleName))
                return Content($"Le rôle '{roleName}' existe déjà.");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
                return Content($"Rôle '{roleName}' créé avec succès !");

            return Content("Erreur lors de la création du rôle.");
        }

        // ----------------------------------------------------------------------
        // 2. ASSIGNER UN RÔLE À UN UTILISATEUR
        // ----------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Content("Utilisateur introuvable.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return Content("Ce rôle n'existe pas.");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
                return Content($"Rôle '{roleName}' assigné à {email}.");

            return Content("Erreur lors de l’assignation du rôle.");
        }

        // ----------------------------------------------------------------------
        // 3. ASSIGNER PLUSIEURS RÔLES
        // ----------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddRoles(string email, string[] roles)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Content("Utilisateur introuvable.");

            var result = await _userManager.AddToRolesAsync(user, roles);

            if (result.Succeeded)
                return Content($"Rôles assignés : {string.Join(", ", roles)}");

            return Content("Erreur lors de l’assignation des rôles.");
        }

        // ----------------------------------------------------------------------
        // 4. VÉRIFIER SI UN UTILISATEUR POSSÈDE UN RÔLE
        // ----------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> IsInRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Content("Utilisateur introuvable.");

            bool hasRole = await _userManager.IsInRoleAsync(user, roleName);

            return Content(hasRole
                ? $"{email} possède le rôle '{roleName}'."
                : $"{email} ne possède PAS le rôle '{roleName}'.");
        }
    }
}