using Microsoft.AspNetCore.Identity;

namespace LetsCommunicate.Infrastructure.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
