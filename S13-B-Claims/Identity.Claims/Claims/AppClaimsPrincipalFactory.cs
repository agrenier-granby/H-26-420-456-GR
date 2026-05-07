using Identity.Claims.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.Claims.Claims
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public const string CLAIM_DATE_INSCRIPTION = "inscription";

        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            return base.CreateAsync(user);
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(CLAIM_DATE_INSCRIPTION, user.DateInscription.ToShortDateString()));

            if (user.Surnom != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.Surnom));
            }

            return identity;
        }
    }
}
