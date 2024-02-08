using LetsCommunicate.Infrastructure.Models.User;

namespace LetsCommunicate.Infrastructure.Models
{
    public class GroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string OwnerEmail { get; set; }

        public List<LoginUserResponse> Users { get; set; }
        public List<MessageResponse> Messages { get; set; }

        public List<string> PermissionEmails { get; set; }
    }
}
