using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [EmailAddress]
        public override string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
