using LetsCommunicate.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.AccountCommand
{
    public class EditUserCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string? UserEmail { get; set; }

        public string UserName { get; set; }
        public string? City { get; set; } = "";
        public string? Country { get; set; } = "";
        public string? Description { get; set; } = "";
    }
}
