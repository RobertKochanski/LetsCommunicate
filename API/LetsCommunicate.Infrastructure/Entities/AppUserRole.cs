﻿using Microsoft.AspNetCore.Identity;

namespace LetsCommunicate.Infrastructure.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
