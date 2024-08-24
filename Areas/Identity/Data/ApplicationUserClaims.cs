﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace NoiSys.Areas.Identity.Data
{
    public class ApplicationUserClaims : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaims(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser account)
        {
            var identity = await base.GenerateClaimsAsync(account);
            identity.AddClaim(new Claim("UserNamaDepan", account.NamaLengkap ?? ""));
            return identity;
        }
    }
}
