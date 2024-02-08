using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Commands.InvitationCommand;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvitationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{groupId}")]
        public async Task<IActionResult> createInvitation(CreateInvitationCommand command, Guid groupId)
        {
            command.SenderEmail = User.GetUserEmail();
            command.GroupId = groupId;
            return await _mediator.Send(command).Process();
        }

        [HttpGet]
        public async Task<IActionResult> getUserInvitations()
        {
            var query = new GetInvitationsQuery(User.GetUserEmail());

            return await _mediator.Send(query).Process();
        }

        [HttpDelete("{InvitationId}")]
        public async Task<IActionResult> Delete(Guid InvitationId)
        {
            return await _mediator.Send(new DeleteInvitationCommand(InvitationId)).Process();
        }

        [HttpPut("{InvitationId}")]
        public async Task<IActionResult> Put(Guid InvitationId)
        {
            return await _mediator.Send(new AcceptInvitationCommand(InvitationId, User.GetUserEmail())).Process();
        }
    }
}
