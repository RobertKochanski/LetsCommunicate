namespace LetsCommunicate.Infrastructure.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public Guid GroupId { get; set; }
    }
}
