using System.Text.Json.Serialization;

namespace SportsVault.Models
{
    public class UserReadDto
    {
        public int CustomerNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        //public DateTime CreatedAt { get; set; }   
    }
}
