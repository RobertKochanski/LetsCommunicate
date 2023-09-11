﻿using Microsoft.AspNetCore.Identity;

namespace LetsCommunicate.Infrastructure.Entities
{
    public class AppUser : IdentityUser<Guid>
    {

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
