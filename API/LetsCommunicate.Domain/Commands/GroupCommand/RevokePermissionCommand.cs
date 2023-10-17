using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LetsCommunicate.Domain.Commands.GroupCommand
{
    public class RevokePermissionCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        public string UserEmail { get; set; }
    }

    public class RevokePermissionCommandHandler : IRequestHandler<RevokePermissionCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RevokePermissionCommandHandler> _logger;

        public RevokePermissionCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<RevokePermissionCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(RevokePermissionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var group = await _dbContext.Groups.Include(x => x.Members).Include(x => x.EmailsPermission).FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find user to revoke permission");
                return Result.BadRequest("Can not find user to grant permission");
            }

            if (group == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find group to revoke permission");
                return Result.BadRequest("Can not find group to grant permission");
            }

            if (group.Members.FirstOrDefault(x => x.Email == request.UserEmail) == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] User is not member in this group");
                return Result.BadRequest("User is not member in this group");
            }

            var oldPermission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.UserEmail == user.Email && x.GroupId == group.Id);

            if (oldPermission == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] User don't have permission");
                return Result.BadRequest("User don't have permission");
            }

            try
            {
                _dbContext.Permissions.Remove(oldPermission);
                group.EmailsPermission.Remove(oldPermission);

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
