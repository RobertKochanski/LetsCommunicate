using LetsCommunicate.Domain.Helpers;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsCommunicate.Domain.Queries
{
    public class GetUserByIdQuery : IRequest<Result<UserInfoResponse>>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserInfoResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<UserInfoResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Id == request.Id);

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
