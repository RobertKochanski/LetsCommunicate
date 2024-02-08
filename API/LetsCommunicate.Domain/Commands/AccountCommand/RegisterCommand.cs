using LetsCommunicate.Domain.Results;
using MediatR;

namespace LetsCommunicate.Domain.Commands.AccountCommand
{
    public class RegisterCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
