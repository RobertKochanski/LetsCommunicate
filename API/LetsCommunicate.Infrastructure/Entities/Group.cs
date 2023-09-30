namespace LetsCommunicate.Infrastructure.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string OwnerEmail { get; set; }

        public List<Message> Messages { get; set; } = new List<Message>();
        public List<AppUser> AppUsers { get; set; } = new List<AppUser>();

    }
}
