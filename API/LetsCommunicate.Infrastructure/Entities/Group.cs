namespace LetsCommunicate.Infrastructure.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string OwnerEmail { get; set; }

        public List<Message> Messages { get; set; } = new List<Message>();
        public List<AppUser> Members { get; set; } = new List<AppUser>();
        public List<Permission> EmailsPermission { get; set; } = new List<Permission>();

    }
}
