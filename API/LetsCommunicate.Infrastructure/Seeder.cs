using LetsCommunicate.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsCommunicate.Infrastructure
{
    public class Seeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public Seeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
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

            var group = new Group
            {
                Name = "General",
                OwnerEmail = admin.Email,
            };

            var permission = new Permission() { UserEmail = admin.Email };

            group.Members.Add(await _userManager.FindByEmailAsync(admin.Email));
            group.EmailsPermission.Add(permission);

            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();
        }
    }
}
