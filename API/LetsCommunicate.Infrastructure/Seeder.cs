using LetsCommunicate.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsCommunicate.Infrastructure
{
    public class Seeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public Seeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRoles()
        {
            if (await _roleManager.Roles.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            await _userManager.CreateAsync(admin, "zaq1@WSX");
            await _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
