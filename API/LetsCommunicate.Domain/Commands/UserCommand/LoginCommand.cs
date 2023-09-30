using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Models;
using MediatR;

namespace LetsCommunicate.Domain.Commands.UserCommand
{
    public class LoginCommand : IRequest<Result<UserResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
