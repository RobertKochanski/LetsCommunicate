using LetsCommunicate.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.GroupCommand
{
    public class CreateGroupCommand : IRequest<Result>
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string? UserEmail { get; set; }

        public CreateGroupCommand(string name, string userEmail)
        {
            Name = name;
            UserEmail = userEmail;
        }
    }
}
