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

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAsync(Guid groupId)
        {
            return await _mediator.Send(new GetGroupQuery(groupId)).Process();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateGroupCommand command)
        {
            command.UserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> Delete(Guid groupId)
        {
            var command = new DeleteGroupCommand(groupId, User.GetUserEmail());

            return await _mediator.Send(command).Process();
        }

        [HttpPut("RemoveFromGroup/{groupId}")]
        public async Task<IActionResult> RemoveFromGroup(RemoveFromGroupCommand command, Guid groupId)
        {
            command.GroupId = groupId;
            command.LoggedUserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }

        [HttpPut("LeaveGroup/{groupId}")]
        public async Task<IActionResult> LeaveGroup(Guid groupId)
        {
            var command = new LeaveGroupCommand(groupId, User.GetUserEmail());

            return await _mediator.Send(command).Process();
        }

        [HttpPut("GrantPermission/{groupId}")]
        public async Task<IActionResult> GrantPermission(GrantPermissionCommand command, Guid groupId)
        {
            command.GroupId = groupId;

            return await _mediator.Send(command).Process();
        }

        [HttpPut("RevokePermission/{groupId}")]
        public async Task<IActionResult> RemovePermission(RevokePermissionCommand command, Guid groupId)
        {
            command.GroupId = groupId;

            return await _mediator.Send(command).Process();
        }
    }
}
