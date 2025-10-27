using Microsoft.AspNetCore.Identity;
using SportsVault.Data;
using SportsVault.Entity;
using SportsVault.Models;
using SportsVault.Services.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace SportsVault.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(CancellationToken ct)
        {
            return await _context.Users
                .Select(u => new UserReadDto
                {
                    CustomerNo = u.CustomerNo,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role
                })
                .ToListAsync(ct);
        }
        public async Task<UserReadDto?> CreateUserAsync (UserCreateDto request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email, ct);
            if (exists)
                return null; // controller will handle as 409 Conflict
            var hashedpassword = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
            var user = new User
            {                
                Email = request.Email,
                Password = hashedpassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Role = "customer",
                IsActive = true
            } ;

            _context.Users.Add(user);
            await _context.SaveChangesAsync(ct);

            return new UserReadDto
            {                
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //PhoneNumber = user.PhoneNumber,
                PhoneNumber = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber,
                Role = user.Role                
            };
        }

        public enum AdminUpgradeResult { NotFound, AlreadyAdmin, Promoted, NotApplicable }

        public async Task<AdminUpgradeResult> AdminUpgradeResultAsync(string email, CancellationToken ct)
        {
            var norm = email.Trim().ToLowerInvariant();

            var user = await _context.Users.SingleOrDefaultAsync(
                u => u.Email.ToLower() == norm, ct);

            if (user is null) return AdminUpgradeResult.NotFound;

            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                return AdminUpgradeResult.AlreadyAdmin;

            if (string.Equals(user.Role, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                user.Role = "admin";
                await _context.SaveChangesAsync(ct);
                return AdminUpgradeResult.Promoted;
            }
            return AdminUpgradeResult.NotApplicable;
        }

        public async Task<bool> DeleteUserByEmailAsync(string email, CancellationToken ct)
        {
            var norm = email.Trim().ToLowerInvariant();
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email.ToLower() == norm, ct);

            if (user is null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
            return true;
        }

    }
}
