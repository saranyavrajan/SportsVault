using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportsVault.Data;
using SportsVault.Entity;
using SportsVault.Models;
using SportsVault.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SportsVault.Services.Implementations
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return null;
            }           
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }
            var at = CreateAccessToken(user);
            //var rt = CreateRefreshToken(user.UserId, GetIp(), GetUserAgent());
            //var rt = CreateRefreshToken(user.UserId);
            var (refreshEntity, rawToken) = CreateRefreshToken(user.UserId);
            context.RefreshToken.Add(refreshEntity);
            await context.SaveChangesAsync();

            return new TokenResponseDto { AccessToken = at, RefreshToken = rawToken };
        }

        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokendescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokendescriptor);
        }
        
        private static (RefreshToken rt, string Raw) CreateRefreshToken(Guid userId)
        //private static RefreshToken CreateRefreshToken(Guid userId, string? ip, string? ua)
        {
            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var rt = new RefreshToken
            {
                UserId = userId,
                //RawToken = raw,
                TokenHash = Hash(raw),
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
                CreatedUtc = DateTime.UtcNow,
                //CreatedByIp = ip,
                //UserAgent = ua
            };
            return (rt, raw);
        }

        private static string Hash(string token) => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var hash = Hash(request.RefreshToken);
            var rt = await context.RefreshToken
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.TokenHash == hash);

            if (rt == null || rt.RevokedUtc != null || rt.ExpiresUtc <= DateTime.UtcNow)
                return null;

            // rotate
            rt.RevokedUtc = DateTime.UtcNow;  // Make old token entry as revoked with current time.
            //var newRt = CreateRefreshToken(rt.UserId, GetIp(), GetUserAgent());
            var (newRt, newRawToken) = CreateRefreshToken(rt.UserId);
            rt.ReplacedByTokenHash = newRt.TokenHash;

            context.RefreshToken.Add(newRt);
            await context.SaveChangesAsync();

            var access = CreateAccessToken(rt.User);
            return new TokenResponseDto { AccessToken = access, RefreshToken = newRawToken};
        }

        //private async Task<TokenResponseDto> CreateTokenResponse(User user)
        //{
        //    return new TokenResponseDto
        //    {
        //        AccessToken = CreateToken(user),
        //        RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        //    };
        //}



        //private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        //{
        //    var refreshToken = GenerateRefreshToken();
        //    user.RefreshToken = refreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        //    await context.SaveChangesAsync();
        //    return refreshToken;
        //}

        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using var rng = RandomNumberGenerator.Create();
        //    rng.GetBytes(randomNumber);
        //    return Convert.ToBase64String(randomNumber);
        //}       

        //public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        //{
        //    var user = await ValidateRefreshTokenAsync(request.Email, request.RefreshToken);
        //    if (user is null)
        //        return null;

        //    return await CreateTokenResponse(user);
        //}

        //private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        //{
        //    var user = await context.Users.FindAsync(userId);
        //    if (user is null || user.RefreshToken != refreshToken
        //        || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        //    {
        //        return null;
        //    }
        //    return user;
        //}
    }
}
