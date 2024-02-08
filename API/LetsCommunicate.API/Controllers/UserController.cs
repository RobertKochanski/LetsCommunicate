using LetsCommunicate.API.Extensions;
using LetsCommunicate.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? searchPhase)
        {
            var query = new GetSearchUsersQuery(searchPhase);
            return await _mediator.Send(query).Process();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var query = new GetUserByIdQuery(Id);
            return await _mediator.Send(query).Process();
        }
    }
}
