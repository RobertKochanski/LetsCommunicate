namespace LetsCommunicate.Infrastructure.Models.User
{
    public class LoginUserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Token { get; set; }
    }
}
