using LetsCommunicate.Domain.Authentication;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LetsCommunicate.Domain.Commands
{
    public class RegisterCommand : IRequest<Result<UserResponse>>
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<UserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);

            if (user != null)
            {
                Result.BadRequest<UserResponse>("User with this email already exist");
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
                return Result.BadRequest<UserResponse>(createResult.Errors.Select(x => x.Description).ToList());
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "Member");

            if (!roleResult.Succeeded) 
            {
                return Result.BadRequest<UserResponse>(roleResult.Errors.Select(x => x.Description).ToList());
            } 

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
