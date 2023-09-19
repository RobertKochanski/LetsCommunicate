namespace LetsCommunicate.Infrastructure.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Message> Messages { get; set; }
    }
}
