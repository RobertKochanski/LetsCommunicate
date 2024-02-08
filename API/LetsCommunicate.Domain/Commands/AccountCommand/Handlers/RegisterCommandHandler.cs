using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.AccountCommand.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<RegisterCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                _logger.LogError($"[{DateTime.Now}] User with this email already exist");
                return Result.BadRequest<LoginUserResponse>("User with this email already exist");
            }

            List<string> errorList = new List<string>();

            if (string.IsNullOrEmpty(request.UserName))
            {
                errorList.Add("Username cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                errorList.Add("Email cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                errorList.Add("Password cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.PasswordConfirm))
            {
                errorList.Add("Confirm password cannot be empty.");
            }
            if (request.Password != request.PasswordConfirm)
            {
                errorList.Add("Password is diffrent than confirm");
            }

            if (errorList.Count > 0)
            {
                foreach (var error in errorList)
                {
                    _logger.LogError($"[{DateTime.Now}] {error}");
                }
                return Result.BadRequest<LoginUserResponse>(errorList);
            }

            AppUser appUser = new AppUser()
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                RegisterDate = DateTime.Now,
            };

            var createResult = await _userManager.CreateAsync(appUser, request.Password);

            if (!createResult.Succeeded)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + createResult.Errors.Select(x => x.Description)));
                return Result.BadRequest<LoginUserResponse>(createResult.Errors.Select(x => x.Description).ToList());
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "Member");

            if (!roleResult.Succeeded)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + roleResult.Errors.Select(x => x.Description)));
                return Result.BadRequest<LoginUserResponse>(roleResult.Errors.Select(x => x.Description).ToList());
            }

            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Name == "General");

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest<LoginUserResponse>("Can not find group");
            }

            group.Members.Add(await _userManager.FindByEmailAsync(appUser.Email));

            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
