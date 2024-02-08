using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Models.User;
using MediatR;

namespace LetsCommunicate.Domain.Commands.AccountCommand
{
    public class LoginCommand : IRequest<Result<LoginUserResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
