namespace SportsVault.Models
{
    public class RefreshTokenRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public required string RefreshToken { get; set; }
    }
}
