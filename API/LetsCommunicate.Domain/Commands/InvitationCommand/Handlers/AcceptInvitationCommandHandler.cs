using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.InvitationCommand.Handlers
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AcceptInvitationCommandHandler> _logger;

        public AcceptInvitationCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<AcceptInvitationCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            var invitation = await _dbContext.Invitations.FirstOrDefaultAsync(x => x.Id == request.InvitationId);

            if (invitation == null)
            {
                _logger.LogError($"Can not find invitation with id: {request.InvitationId}");
                return Result.NotFound(request.InvitationId);
            }

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest("Can not find user");
            }

            var group = await _dbContext.Groups
                .FirstOrDefaultAsync(x => x.Id == invitation.GroupId);

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest("Can not find group");
            }

            group.Members.Add(user);
            _dbContext.Invitations.Remove(invitation);

            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
