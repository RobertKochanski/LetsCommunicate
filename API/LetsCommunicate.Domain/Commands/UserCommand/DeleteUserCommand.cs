using LetsCommunicate.Domain.Results;
using MediatR;

namespace LetsCommunicate.Domain.Commands.UserCommand
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
