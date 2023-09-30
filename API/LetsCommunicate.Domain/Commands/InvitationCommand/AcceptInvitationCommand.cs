using LetsCommunicate.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.InvitationCommand
{
    public class AcceptInvitationCommand : IRequest<Result>
    {
        public Guid InvitationId { get; set; }
        [JsonIgnore]
        public string? UserEmail { get; set; }

        public AcceptInvitationCommand(Guid invitationId, string userEmail)
        {
            InvitationId = invitationId;
            UserEmail = userEmail;
        }
    }
}
