namespace LetsCommunicate.Infrastructure.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string? PublicId { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
