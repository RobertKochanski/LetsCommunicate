using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Commands.GroupCommand.Handlers
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CreateGroupCommandHandler> _logger;

        public CreateGroupCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<CreateGroupCommandHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                _logger.LogError($"[{DateTime.Now}] Fill group name");
                return Result.BadRequest("Fill group name");
            }

            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest("Can not find user");
            }

            var group = new Group()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OwnerEmail = request.UserEmail!,
            };

            var permission = new Permission() { UserEmail = user.Email, GroupId = group.Id };

            try
            {
                group.Members.Add(user);
                group.EmailsPermission.Add(permission);
                await _dbContext.Permissions.AddAsync(permission);
                await _dbContext.Groups.AddAsync(group);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(" ", $"[{DateTime.Now}]" + ex.Message));
                return Result.BadRequest(ex.Message);
            }

            return Result.Ok();
        }
    }
}
