using LetsCommunicate.Domain.Helpers;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Queries
{
    public class GetMyInfoQuery : IRequest<Result<UserInfoResponse>>
    {
        public string UserEmail { get; set; }

        public GetMyInfoQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
    }

    public class GetMyInfoQueryHandler : IRequestHandler<GetMyInfoQuery, Result<UserInfoResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<GetMyInfoQueryHandler> _logger;

        public GetMyInfoQueryHandler(UserManager<AppUser> userManager, ILogger<GetMyInfoQueryHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<UserInfoResponse>> Handle(GetMyInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Email == request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest<UserInfoResponse>("Can not find user");
            }

            var response = new UserInfoResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Description = user.Description,
                City = user.City,
                Country = user.Country,
                RegisterDate = user.RegisterDate,
                DateOfBirth = user.DateOfBirth,
                Age = CalculateUserAge.CalculateAge(user.DateOfBirth),
                PhotoUrl = user.Photo != null ? user.Photo.Url : null,
            };
            

            return Result.Ok(response);
        }
    }
}
