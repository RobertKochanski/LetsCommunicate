using LetsCommunicate.Domain.Authentication;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.AccountCommand.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginUserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, ILogger<LoginCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<LoginUserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Please fill all fields");
                return Result.BadRequest<LoginUserResponse>("Please fill all fields");
            }

            var user = await _userManager.Users.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Wrong email or password");
                return Result.BadRequest<LoginUserResponse>("Wrong email or password");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Wrong email or password");
                return Result.BadRequest<LoginUserResponse>("Wrong email or password");
            }

            var response = new LoginUserResponse();

            if (user.Photo != null)
            {
                response = new LoginUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhotoUrl = user.Photo.Url,
                    Token = await _tokenService.CreateToken(user),
                };
            }
            else
            {
                response = new LoginUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhotoUrl = null,
                    Token = await _tokenService.CreateToken(user),
                };
            }

            return Result.Ok(response);
        }
    }
}
