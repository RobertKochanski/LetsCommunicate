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
    public class LeaveGroupCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public string? UserEmail { get; set; }

        public LeaveGroupCommand(Guid groupId, string? userEmail)
        {
            GroupId = groupId;
            UserEmail = userEmail;
        }
    }

    public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<LeaveGroupCommandHandler> _logger;

        public LeaveGroupCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<LeaveGroupCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var group = await _dbContext.Groups.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == request.GroupId);

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

            if (!group.Members.Contains(user))
            {
                _logger.LogError($"[{DateTime.UtcNow}] User is not member in this group");
                return Result.BadRequest("User is not member in this group");
            }

            try
            {
                var permissionCheck = group.EmailsPermission.FirstOrDefault(x => x.UserEmail == user.Email);

                if (permissionCheck != null)
                {
                    group.EmailsPermission.Remove(permissionCheck);
                }

                group.Members.Remove(user);
                await _dbContext.SaveChangesAsync();
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
