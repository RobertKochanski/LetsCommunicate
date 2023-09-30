using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using LetsCommunicate.Infrastructure.Models;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Commands.UserCommand.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<DeleteUserCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest("Can not find user");
            }

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + deleteResult.Errors.Select(x => x.Description)));
                return Result.BadRequest<UserResponse>(deleteResult.Errors.Select(x => x.Description).ToList());
            }

            await _dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
