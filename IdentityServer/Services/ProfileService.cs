using IdentityModel;
using IdentityServer.Data.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;
        public ProfileService(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            var user = await _userManager.GetUserAsync(context.Subject);
            var rolenameCollection = await _userManager.GetRolesAsync(user);
            var rolename = rolenameCollection.SingleOrDefault();

            var claims = new List<Claim>();

            if (user.Email != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            }
            if (user.UserName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            }

            if (user.FirstName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }

            if (user.LastName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }

            context.IssuedClaims.AddRange(claims);


        }

        public async Task IsActiveAsync(IsActiveContext context)
        {

            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = (user != null);

        }
    }
}
