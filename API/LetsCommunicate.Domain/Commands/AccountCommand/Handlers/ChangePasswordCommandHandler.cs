using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using LetsCommunicate.Infrastructure.Models.User;

namespace LetsCommunicate.Domain.Commands.AccountCommand.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<ChangePasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Problem with find user to change password");
                return Result.BadRequest<LoginUserResponse>("Problem with find user to change password");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
            {
                _logger.LogError($"[{DateTime.Now}] Incorrect old password");
                return Result.BadRequest<LoginUserResponse>("Incorrect old password");
            }

            if (request.OldPassword == request.NewPassword)
            {
                _logger.LogError($"[{DateTime.Now}] New password can not be the same as old password");
                return Result.BadRequest<LoginUserResponse>("New password can not be the same as old password");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                _logger.LogError($"[{DateTime.Now}] New password is diffrent than confirm password");
                return Result.BadRequest<LoginUserResponse>("New Password is diffrent than confirm password");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                _logger.LogError(string.Join(" ", result.Errors.Select(x => x.Description)));
                return Result.BadRequest<string>(result.Errors.Select(x => x.Description).ToList());
            }

            return Result.Ok();
        }
    }
}
