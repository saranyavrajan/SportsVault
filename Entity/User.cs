using System.ComponentModel.DataAnnotations;

namespace SportsVault.Entity
{
    public class User
    {
        [Key]
        [Required]        
        public Guid UserId {  get; set; } = Guid.NewGuid(); // Ef Core generates the Id

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MinLength(1, ErrorMessage = "Enter a valid email id")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        public int CustomerNo { get; private set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one number.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50, ErrorMessage = "Max Length for First Name is 50 characters")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "First Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50, ErrorMessage = "Max Length for Last Name is 50 characters")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Last Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        //[RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in valid international format")]
        [RegularExpression(@"^(?:\+?[1-9]\d{1,14})?$", ErrorMessage = "Phone number must be in valid international format: optional '+' at the start, followed by 2–15 digits with no spaces, letters, or leading zero. Like +14155552671")]
        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(admin|customer)$", ErrorMessage = "Role must be either admin or customer")]
        public string Role { get; set; } = "customer";

        [Required]
        public Boolean IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;     
    }
}
