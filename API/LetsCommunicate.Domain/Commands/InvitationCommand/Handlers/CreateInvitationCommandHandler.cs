using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Commands.InvitationCommand.Handlers
{
    public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CreateInvitationCommandHandler> _logger;

        public CreateInvitationCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<CreateInvitationCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            var invitationCheck = await _dbContext.Invitations.Where(x => x.GroupId == request.GroupId && x.InvitedEmail == request.InvitedEmail).FirstOrDefaultAsync();

            if (invitationCheck != null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Invitation already exist");
                return Result.BadRequest("Invitation already exist");
            }

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest("Can not find group");
            }

            if (group.Members.Contains(await _userManager.FindByEmailAsync(request.InvitedEmail)))
            {
                _logger.LogError($"[{DateTime.Now}] User is already in the group");
                return Result.BadRequest("User is already in the group");
            }

            if (await _userManager.FindByEmailAsync(request.SenderEmail) == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find sender email");
                return Result.BadRequest("Can not find sender");
            }

            if (await _userManager.FindByEmailAsync(request.InvitedEmail) == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find invited email");
                return Result.BadRequest("Can not find user");
            }

            var invitation = new Invitation()
            {
                SenderEmail = request.SenderEmail,
                InvitedEmail = request.InvitedEmail,
                GroupId = request.GroupId,
                Group = group,
                InvitedAt = DateTime.Now,
            };

            await _dbContext.Invitations.AddAsync(invitation);
            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
