using LetsCommunicate.Infrastructure.Entities;

namespace LetsCommunicate.Domain.Authentication
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
