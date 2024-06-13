using IdentityModel;
using IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Data.DbSeed
{
    public class DbSeed : IDbSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeed(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Seed()
        {
            if(_roleManager.FindByIdAsync(Config.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Config.User)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }

            var admin = new ApplicationUser()
            {
                UserName = "admin",
                FirstName = "adminFirst",
                LastName = "adminLast",
                Email = "testAdmin@gmail.com",
                EmailConfirmed = true,
            };

            _userManager.CreateAsync(admin, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(admin, Config.Admin).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(admin, new Claim[]
             {
                    new Claim(JwtClaimTypes.Name, admin.UserName),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, Config.Admin),
             }).Result;

            var user = new ApplicationUser()
            {
                UserName = "user",
                FirstName = "userFirst",
                LastName = "userLast",
                Email = "testUser@gmail.com",
                EmailConfirmed = true,

            };

            _userManager.CreateAsync(user, "testUser123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, Config.User).GetAwaiter().GetResult();

            var temp3 = _userManager.AddClaimsAsync(user, new Claim[]
             {
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                    new Claim(JwtClaimTypes.Role, Config.User),
             }).Result;

        }
    }
}
