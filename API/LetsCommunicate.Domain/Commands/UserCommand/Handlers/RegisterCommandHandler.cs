using LetsCommunicate.Domain.Authentication;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.UserCommand.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ITokenService tokenService, ILogger<RegisterCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<UserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);

            if (user != null)
            {
                _logger.LogError($"[{DateTime.Now}] User with this email already exist");
                return Result.BadRequest<UserResponse>("User with this email already exist");
            }

            List<string> errorList = new List<string>();

            if (string.IsNullOrEmpty(request.userName))
            {
                errorList.Add("Username cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.email))
            {
                errorList.Add("Email cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.password))
            {
                errorList.Add("Password cannot be empty.");
            }
            if (string.IsNullOrEmpty(request.passwordConfirm))
            {
                errorList.Add("Confirm password cannot be empty.");
            }
            if (request.password != request.passwordConfirm)
            {
                errorList.Add("Password is diffrent than confirm");
            }

            if (errorList.Count > 0)
            {
                foreach (var error in errorList)
                {
                    _logger.LogError($"[{DateTime.Now}] {error}");
                }
                return Result.BadRequest<UserResponse>(errorList);
            }

            AppUser appUser = new AppUser()
            {
                Id = Guid.NewGuid(),
                UserName = request.userName,
                Email = request.email,
            };

            var createResult = await _userManager.CreateAsync(appUser, request.password);

            if (!createResult.Succeeded)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + createResult.Errors.Select(x => x.Description)));
                return Result.BadRequest<UserResponse>(createResult.Errors.Select(x => x.Description).ToList());
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "Member");

            if (!roleResult.Succeeded)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + roleResult.Errors.Select(x => x.Description)));
                return Result.BadRequest<UserResponse>(roleResult.Errors.Select(x => x.Description).ToList());
            }

            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Name == "General");

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest<UserResponse>("Can not find group");
            }

            group.Members.Add(await _userManager.FindByEmailAsync(appUser.Email));

            await _dbContext.SaveChangesAsync();

            UserResponse userResponse = new UserResponse()
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = await _tokenService.CreateToken(appUser)
            };

            return Result.Ok(userResponse);
        }
    }
}
