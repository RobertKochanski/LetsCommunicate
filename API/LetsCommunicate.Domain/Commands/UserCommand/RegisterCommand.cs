using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Models;
using MediatR;

namespace LetsCommunicate.Domain.Commands.UserCommand
{
    public class RegisterCommand : IRequest<Result<UserResponse>>
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
    }
}
