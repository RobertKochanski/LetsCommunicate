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
    public class RemoveFromGroupCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public string? LoggedUserEmail { get; set; }
        public string UserEmail { get; set; }

        public RemoveFromGroupCommand(Guid groupId, string userEmail)
        {
            GroupId = groupId;
            UserEmail = userEmail;
        }
    }

    public class RemoveFromGroupCommandHandler : IRequestHandler<RemoveFromGroupCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RemoveFromGroupCommandHandler> _logger;

        public RemoveFromGroupCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<RemoveFromGroupCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(RemoveFromGroupCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var loggedUser = await _userManager.FindByEmailAsync(request.LoggedUserEmail);
            var group = await _dbContext.Groups.Include(x => x.Members).Include(x => x.EmailsPermission).FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find user");
                return Result.BadRequest("Can not find user");
            }

            if (group == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find group");
                return Result.BadRequest("Can not find group");
            }

            if (group.EmailsPermission.FirstOrDefault(x => x.UserEmail == loggedUser.Email) == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] You don't have permission");
                return Result.BadRequest("You don't have permission");
            }

            if (!group.Members.Contains(user))
            {
                _logger.LogError($"[{DateTime.UtcNow}] User is not member in group");
                return Result.BadRequest("User is not member in group");
            }

            try
            {
                var permissionCheck = group.EmailsPermission.FirstOrDefault(x => x.UserEmail == user.Email);

                if (permissionCheck != null)
                {
                    group.EmailsPermission.Remove(permissionCheck);
                }

                group.Members.Remove(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest(ex.Message);
            }

            return Result.Ok();
        }
    }
}
