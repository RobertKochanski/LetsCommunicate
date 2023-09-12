using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Commands;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            return await _mediator.Send(command).Process();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return await _mediator.Send(command).Process();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetAllUsersQuery();
            return await _mediator.Send(query).Process();
        }
    }
}
