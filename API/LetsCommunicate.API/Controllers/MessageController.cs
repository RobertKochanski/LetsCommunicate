using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Commands.MessageCommand;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupMessages([FromRoute] Guid groupId)
        {
            var query = new GetGroupMessageQuery(groupId);
            return await _mediator.Send(query).Process();
        }

        [HttpPost("{groupId}")]
        public async Task<IActionResult> CreateMessage(CreateMessageCommand command, Guid groupId)
        {
            command.GroupId = groupId;
            command.SenderEmail = User.GetUserEmail();
            return await _mediator.Send(command).Process();
        }
    }
}
