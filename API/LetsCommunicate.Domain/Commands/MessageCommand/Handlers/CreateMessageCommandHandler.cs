using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.MessageCommand.Handlers
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CreateMessageCommandHandler> _logger;

        public CreateMessageCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<CreateMessageCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _userManager.FindByEmailAsync(request.SenderEmail);

            if (sender == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest("Can not find user");
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                _logger.LogError($"[{DateTime.Now}] Fill content field");
                return Result.BadRequest("Fill content field");
            }

            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if (group == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find group");
                return Result.BadRequest("Can not find group");
            }

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Sender = sender,
                SenderId = sender.Id,
                GroupId = group.Id,
                Group = group,
                MessageSent = DateTime.Now
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
