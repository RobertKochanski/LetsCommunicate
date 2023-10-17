using MediatR;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Commands.InvitationCommand
{
    public class DeleteInvitationCommand : IRequest<Result>
    {
        public Guid InvitationId { get; set; }

        public DeleteInvitationCommand(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}
