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
    public class DeleteGroupCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public string UserEmail { get; set; }

        public DeleteGroupCommand(Guid groupId, string userEmail)
        {
            GroupId = groupId;
            UserEmail = userEmail;
        }
    }

    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DeleteGroupCommandHandler> _logger;

        public DeleteGroupCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<DeleteGroupCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find user to delete group");
                return Result.BadRequest("Can not find user to delete group");
            }

            if (group == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find group to delete");
                return Result.BadRequest("Can not find group to delete");
            }

            if (group.OwnerEmail != user.Email)
            {
                _logger.LogError($"[{DateTime.UtcNow}] You dont have permission to delete this group");
                return Result.BadRequest("You dont have permission to delete this group");
            }

            try
            {
                _dbContext.Groups.Remove(group);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest(ex.Message);
            }

            return Result.NoContent();
        }
    }
}
