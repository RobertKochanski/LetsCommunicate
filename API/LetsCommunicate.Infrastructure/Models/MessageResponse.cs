namespace LetsCommunicate.Infrastructure.Models
{
    public class MessageResponse
    {
        public Guid SenderId { get; set; }
        public UserResponse Sender { get; set; }

        public Guid GroupId { get; set; }

        public string Content { get; set; }
        public DateTime? MessageSent { get; set; } = DateTime.UtcNow;
    }
}
