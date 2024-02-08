using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Commands.AccountCommand;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            return await _mediator.Send(command).Process();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return await _mediator.Send(command).Process();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit(EditUserCommand command)
        {
            command.UserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            command.UserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            var commnad = new DeleteUserCommand(User.GetUserGuid());
            return await _mediator.Send(commnad).Process();
        }

        [HttpGet("MyInfo")]
        public async Task<IActionResult> GetMyInfo()
        {
            var query = new GetMyInfoQuery(User.GetUserEmail());
            return await _mediator.Send(query).Process();
        }

        [HttpPut("ChangePhoto")]
        public async Task<IActionResult> changePhoto(IFormFile file)
        {
            var command = new ChangePhotoCommand(file);
            command.UserEmail = User.GetUserEmail();

            return await _mediator.Send(command).Process();
        }
    }
}
