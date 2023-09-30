using LetsCommunicate.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LetsCommunicate.Domain.Commands.MessageCommand
{
    public class CreateMessageCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string? SenderEmail { get; set; }
        [JsonIgnore]
        public Guid? GroupId { get; set; }
        public string Content { get; set; }

        public CreateMessageCommand(string content)
        {
            Content = content;
        }
    }
}
