#nullable disable
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BoardGameBrawl.Data
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AppUserClaimsPrincipalFactory(
         UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager,
         IOptions<IdentityOptions> optionsAccessor)
         : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim("FirstName", user.FirstName)
            });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim("LastName", user.LastName)
            });
            }

            if (!string.IsNullOrWhiteSpace(user.BGGUsername))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim("BGGUsername", user.BGGUsername)
            });
            }

            identity.AddClaims(claims);
            return principal;
        }
    }
}
