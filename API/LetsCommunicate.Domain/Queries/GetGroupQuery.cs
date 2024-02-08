using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Models.User;

namespace LetsCommunicate.Domain.Queries
{
    public class GetGroupQuery : IRequest<Result<GroupResponse>>
    {
        public Guid Id { get; set; }

        public GetGroupQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, Result<GroupResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetGroupQueryHandler> _logger;

        public GetGroupQueryHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<GetGroupQueryHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<GroupResponse>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                var groupGeneral = await _dbContext.Groups
                .Include(x => x.Messages)
                    .ThenInclude(x => x.Sender)
                        .ThenInclude(x => x.Photo)
                .Include(x => x.EmailsPermission)
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Name == "General");

                var general = new GroupResponse()
                {
                    Id = groupGeneral.Id,
                    Name = groupGeneral.Name,
                    OwnerEmail = groupGeneral.OwnerEmail,

                    Messages = groupGeneral.Messages
                    .OrderByDescending(x => x.MessageSent)
                    .Select(x => new MessageResponse()
                    {
                        Content = x.Content,
                        GroupId = x.GroupId,
                        Sender = new LoginUserResponse()
                        {
                            Id = x.Sender.Id,
                            Email = x.Sender.Email,
                            UserName = x.Sender.UserName,
                            PhotoUrl = x.Sender.Photo?.Url,
                            Token = null
                        },
                        MessageSent = x.MessageSent,
                    }).ToList(),

                    Users = groupGeneral.Members.Select(x => new LoginUserResponse()
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        Token = null,
                        PhotoUrl = x.Photo?.Url,
                    }).ToList(),

                    PermissionEmails = groupGeneral.EmailsPermission
                    .Select(x => new string(x.UserEmail))
                    .ToList()
                };

                return Result.Ok(general);
            }

            var group = await _dbContext.Groups
                .Include(x => x.Messages)
                    .ThenInclude(x => x.Sender)
                        .ThenInclude(x => x.Photo)
                .Include(x => x.EmailsPermission)
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (group == null)
            {
                _logger.LogError($"[{DateTime.UtcNow}] Can not find group");
                return Result.BadRequest<GroupResponse>("Can not find group");
            }

            var response = new GroupResponse()
            {
                Id = group.Id,
                Name = group.Name,
                OwnerEmail = group.OwnerEmail,

                Messages = group.Messages
                    .OrderByDescending(x => x.MessageSent)
                    .Select(x => new MessageResponse()
                {
                    Content = x.Content,
                    GroupId = x.GroupId,
                    Sender = new LoginUserResponse()
                    {
                        Id = x.Sender.Id,
                        Email = x.Sender.Email,
                        UserName = x.Sender.UserName,
                        PhotoUrl = x.Sender.Photo?.Url,
                        Token = null
                    },
                    MessageSent = x.MessageSent,
                }).ToList(),

                Users = group.Members.Select(x => new LoginUserResponse()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    Token = null,
                    PhotoUrl = x.Photo?.Url,
                }).ToList(),

                PermissionEmails = group.EmailsPermission
                    .Select(x => new string(x.UserEmail))
                    .ToList()
            };

            try
            {
                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest<GroupResponse>(ex.Message);
            }
        }
    }
}
