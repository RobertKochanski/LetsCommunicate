﻿using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure;
using LetsCommunicate.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetsCommunicate.Domain.Queries
{
    public class GetInvitationsQuery : IRequest<Result<List<Invitation>>>
    {
        public string UserEmail { get; set; }

        public GetInvitationsQuery(string userEmail)
        {
            UserEmail = userEmail;
        }
    }

    public class GetInvitationsQueryHandler : IRequestHandler<GetInvitationsQuery, Result<List<Invitation>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GetInvitationsQueryHandler> _logger;

        public GetInvitationsQueryHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<GetInvitationsQueryHandler> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<List<Invitation>>> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Can not find user");
                return Result.BadRequest<List<Invitation>>("Can not find user");
            }

            var invitations = await _dbContext.Invitations
                .Include(x => x.Group)
                .Where(x => x.InvitedEmail.ToLower() == user.Email.ToLower())
                .ToListAsync();

            return Result.Ok(invitations);
        }
    }
}
