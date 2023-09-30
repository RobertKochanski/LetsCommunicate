using MediatR;
using System.Text.Json.Serialization;
using LetsCommunicate.Domain.Results;

namespace LetsCommunicate.Domain.Commands.InvitationCommand
{
    public class CreateInvitationCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string? SenderEmail { get; set; }
        public string InvitedEmail { get; set; }
        [JsonIgnore]
        public Guid GroupId { get; set; }

        public CreateInvitationCommand(string senderEmail, string invitedEmail, Guid groupId)
        {
            SenderEmail = senderEmail;
            InvitedEmail = invitedEmail;
            GroupId = groupId;
        }
    }
}
