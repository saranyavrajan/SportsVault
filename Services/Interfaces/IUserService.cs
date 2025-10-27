using SportsVault.Entity;
using SportsVault.Models;
using static SportsVault.Services.Implementations.UserService;

namespace SportsVault.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(CancellationToken ct);
        Task<UserReadDto?> CreateUserAsync(UserCreateDto request, CancellationToken ct);
        Task<AdminUpgradeResult> AdminUpgradeResultAsync(string email, CancellationToken ct);
        Task<bool> DeleteUserByEmailAsync(string email, CancellationToken ct);

    }
}
