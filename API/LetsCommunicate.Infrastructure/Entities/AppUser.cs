using Microsoft.AspNetCore.Identity;

namespace LetsCommunicate.Infrastructure.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? City { get; set; } = "";
        public string? Country { get; set; } = "";
        public string? Description { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; }

        public Photo? Photo { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
