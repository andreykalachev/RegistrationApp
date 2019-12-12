using RegistrationApp.Domain.Core.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace RegistrationApp.Domain.Services.Utilities
{
    internal static class ClaimsGenerator
    {
        internal static ClaimsPrincipal GenerateClaim(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return new ClaimsPrincipal(identity);
        }
    }
}
