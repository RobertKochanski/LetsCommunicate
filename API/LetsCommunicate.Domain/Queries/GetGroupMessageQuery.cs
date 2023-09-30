using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Queries
{
    public class GetGroupMessageQuery : IRequest<Result<List<MessageResponse>>>
    {
        public Guid GroupId { get; set; }

        public GetGroupMessageQuery(Guid groupId)
        {
            GroupId = groupId;
        }
    }

    public class GetGroupMessageHandler : IRequestHandler<GetGroupMessageQuery, Result<List<MessageResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetGroupMessageHandler> _logger;

        public GetGroupMessageHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<GetGroupMessageHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<List<MessageResponse>>> Handle(GetGroupMessageQuery request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest<List<MessageResponse>>("Can not find group");
            }

            var messages = await _dbContext.Messages
                .Include(x => x.Sender)
                .Where(x => x.GroupId == group.Id).ToListAsync();

            var response = messages.Select(x => new MessageResponse()
            {
                GroupId = group.Id,
                SenderId = x.SenderId,
                Sender = new UserResponse()
                {
                    Id = x.Sender.Id,
                    Email = x.Sender.Email,
                    UserName = x.Sender.UserName,
                    Token = null
                },
                Content = x.Content,
                MessageSent = x.MessageSent,
            }).ToList();

            return Result.Ok(response);
        }
    }
}
