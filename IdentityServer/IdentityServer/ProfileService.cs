using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer
{
    public class ProfileService : IProfileService
    {
        protected UserManager<IdentityUser> _userManager;

        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName)
                };

            if (_userManager.SupportsUserRole)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
            }

            context.IssuedClaims.AddRange(claims);

            //>Return
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = (user != null);

            //>Return
            return Task.FromResult(0);
        }
    }
}
