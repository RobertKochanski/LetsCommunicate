using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.GroupCommand
{
    public class GrantPermissionCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        public string UserEmail { get; set; }
    }

    public class GrantPermissionCommandHandler : IRequestHandler<GrantPermissionCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GrantPermissionCommandHandler> _logger;

        public GrantPermissionCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<GrantPermissionCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(GrantPermissionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var group = await _dbContext.Groups.Include(x => x.Members).Include(x => x.EmailsPermission).FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find user to grant permission");
                return Result.BadRequest("Can not find user to grant permission");
            }

            if (group == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find group to grant permission");
                return Result.BadRequest("Can not find group to grant permission");
            }

            var newPermission = new Permission() { UserEmail = request.UserEmail, GroupId = group.Id };

            if (group.Members.FirstOrDefault(x => x.Email == request.UserEmail) == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] User is not member in this group");
                return Result.BadRequest("User is not member in this group");
            }

            if (group.EmailsPermission.FirstOrDefault(x => x.UserEmail == request.UserEmail) != null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] User already have permission");
                return Result.BadRequest("User already have permission");
            }

            try
            {
                await _dbContext.Permissions.AddAsync(newPermission);
                group.EmailsPermission.Add(newPermission);

                await _dbContext.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest(ex.Message);
            }
        }
    }
}
