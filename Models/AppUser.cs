using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        // Relationship with Portfolio which holds the many to many relationship between Stock and AppUser
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}