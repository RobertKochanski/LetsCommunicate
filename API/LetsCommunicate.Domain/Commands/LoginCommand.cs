using LetsCommunicate.Domain.Authentication;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands
{
    public class LoginCommand : IRequest<Result<UserResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserResponse>>
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

        public async Task<Result<UserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Please fill all fields");
                return Result.BadRequest<UserResponse>("Please fill all fields");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Wrong email or password");
                return Result.BadRequest<UserResponse>("Wrong email or password");
            }

            if(!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Wrong email or password");
                return Result.BadRequest<UserResponse>("Wrong email or password");
            }

            var response = new UserResponse()
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
            };

            return Result.Ok(response);
        }
    }
}
