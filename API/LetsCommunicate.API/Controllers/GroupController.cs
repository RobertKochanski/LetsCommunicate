using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Commands.GroupCommand;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetGroupsQuery(User.GetUserEmail());
            return await _mediator.Send(query).Process();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAsync(Guid id)
        {
            return await _mediator.Send(new GetGroupQuery(id)).Process();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateGroupCommand command)
        {
            command.UserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }
    }
}
