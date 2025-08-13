using Salon.Domain.Users.Entities;

namespace Salon.Application.Users.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}