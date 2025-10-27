using SportsVault.Entity;

namespace SportsVault.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
