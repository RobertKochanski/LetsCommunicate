using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using LetsCommunicate.Infrastructure.Models.User;

namespace LetsCommunicate.Domain.Commands.AccountCommand.Handlers
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<EditUserCommandHandler> _logger;

        public EditUserCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<EditUserCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Problem with find user to edit");
                return Result.BadRequest<LoginUserResponse>("Problem with find user to edit");
            }

            if (string.IsNullOrEmpty(request.UserName))
            {
                _logger.LogError($"[{DateTime.Now}] Username can not be empty");
                return Result.BadRequest<LoginUserResponse>("Username can not be empty");
            }
            // ####### IDK if username will be unique ##########
            //else if(await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName) != null)
            //{
            //    _logger.LogError($"[{DateTime.Now}] Username already taken");
            //    return Result.BadRequest<LoginUserResponse>("Username already taken");
            //}
            else
            {
                user.UserName = request.UserName;
            }


            if (!string.IsNullOrEmpty(request.Description))
            {
                user.Description = request.Description;
            }
            if (!string.IsNullOrEmpty(request.Country))
            {
                user.Country = request.Country;
            }
            if (!string.IsNullOrEmpty(request.City))
            {
                user.City = request.City;
            }

            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
