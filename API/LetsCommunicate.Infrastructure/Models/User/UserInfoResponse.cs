namespace LetsCommunicate.Infrastructure.Models.User
{
    public class UserInfoResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
