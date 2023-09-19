namespace LetsCommunicate.Infrastructure.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }
        public AppUser Sender { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public string Content { get; set; }
        public DateTime? MessageSent { get; set; } = DateTime.UtcNow;
    }
}
