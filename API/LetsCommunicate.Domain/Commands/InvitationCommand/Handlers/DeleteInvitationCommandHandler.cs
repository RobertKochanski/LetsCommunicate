using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Commands.InvitationCommand.Handlers
{
    public class DeleteInvitationCommandHandler : IRequestHandler<DeleteInvitationCommand, Result>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DeleteInvitationCommandHandler> _logger;

        public DeleteInvitationCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteInvitationCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await _dbContext.Invitations.FirstOrDefaultAsync(x => x.Id == request.InvitationId);

            if (invitation == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find invitation");
                return Result.BadRequest("Can not find invitation");
            }

            try
            {
                _dbContext.Invitations.Remove(invitation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(Environment.NewLine, ex.Message));
                return Result.BadRequest(ex.Message);
            }

            return Result.NoContent();
        }
    }
}
