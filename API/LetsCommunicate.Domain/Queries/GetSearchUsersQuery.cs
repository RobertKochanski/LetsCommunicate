using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsCommunicate.Domain.Queries
{
    public class GetSearchUsersQuery : IRequest<Result<List<SearchUserResponse>>>
    {
        public string? SearchPhase { get; set; }

        public GetSearchUsersQuery(string searchPhase)
        {
            SearchPhase = searchPhase;
        }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetSearchUsersQuery, Result<List<SearchUserResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<List<SearchUserResponse>>> Handle(GetSearchUsersQuery request, CancellationToken cancellationToken)
        {
            var users = new List<SearchUserResponse>();

            if (string.IsNullOrEmpty(request.SearchPhase))
            {
                users = await _userManager.Users
                    .Include(x => x.Photo)
                    .Select(x => new SearchUserResponse()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        PhotoUrl = x.Photo != null ? x.Photo.Url : null,
                    }).ToListAsync();

                return Result.Ok(users);
            }
            else
            {
                users = await _userManager.Users
                    .Include(x => x.Photo)
                    .Where(x => x.Email.ToLower().Contains(request.SearchPhase.ToLower()) || x.UserName.ToLower().Contains(request.SearchPhase.ToLower()))
                    .Distinct()
                    .Select(x => new SearchUserResponse()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        PhotoUrl = x.Photo != null ? x.Photo.Url : null,
                    }).ToListAsync();

                return Result.Ok(users);
            }
        }
    }
}
