using LetsCommunicate.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.AccountCommand
{
    public class ChangePasswordCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string? UserEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
