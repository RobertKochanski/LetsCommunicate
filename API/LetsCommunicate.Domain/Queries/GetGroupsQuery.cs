using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure.Models;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Queries
{
    public class GetGroupsQuery : IRequest<Result<List<GroupResponse>>>
    {
        [JsonIgnore]
        public string UserEmail { get; set; }

        public GetGroupsQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
    }

    public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, Result<List<GroupResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetGroupsQueryHandler> _logger;

        public GetGroupsQueryHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<GetGroupsQueryHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<List<GroupResponse>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find user");
                return Result.BadRequest<List<GroupResponse>>("Can not find user");
            }

            var groups = _dbContext.Groups
                .Include(x => x.Messages)
                    .ThenInclude(x => x.Sender)
                        .ThenInclude(x => x.Photo)
                .Where(x => x.Members.Contains(user))
                .Select(x => new GroupResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerEmail = x.OwnerEmail,
                    Users = x.Members.Select(x => new LoginUserResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        UserName = x.UserName,
                        PhotoUrl = x.Photo != null ? x.Photo.Url : null,
                        Token = null
                    }).ToList(),
                    Messages = x.Messages.Select(x => new MessageResponse()
                    {
                        Content = x.Content,
                        GroupId = x.GroupId,
                        SenderId = x.SenderId,
                        Sender = new LoginUserResponse()
                        {
                            Id = x.Sender.Id,
                            Email = x.Sender.Email,
                            UserName = x.Sender.UserName,
                            PhotoUrl = x.Sender.Photo != null ? x.Sender.Photo.Url : null,
                            Token = null
                        },
                        MessageSent = x.MessageSent
                    }).OrderByDescending(x => x.MessageSent).ToList(),

                    PermissionEmails = x.EmailsPermission
                    .Select(x => new string(x.UserEmail))
                    .ToList()
                })
                .OrderBy(x => x.Name)
                .AsNoTracking();

            try
            {
                var response = await groups.ToListAsync();
                return Result.Ok(await groups.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest<List<GroupResponse>>(ex.Message);
            }
        }
    }
}
