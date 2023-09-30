﻿namespace LetsCommunicate.Infrastructure.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
    }
}
