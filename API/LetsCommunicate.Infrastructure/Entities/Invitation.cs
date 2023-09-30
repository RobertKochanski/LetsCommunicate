namespace LetsCommunicate.Infrastructure.Entities
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public string SenderEmail { get; set; }
        public string InvitedEmail { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime InvitedAt { get; set; } = DateTime.Now;
    }
}
