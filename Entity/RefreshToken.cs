namespace SportsVault.Entity
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string TokenHash { get; set; } = null!; // store hash, not raw
        public DateTime ExpiresUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        //public string? CreatedByIp { get; set; }
        //public string? UserAgent { get; set; }
        public DateTime? RevokedUtc { get; set; }
        public string? ReplacedByTokenHash { get; set; } // for rotation chains
    }
}
