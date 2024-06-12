using IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;

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
            throw new NotImplementedException();
        }
    }
}
