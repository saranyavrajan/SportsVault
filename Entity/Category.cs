using System.ComponentModel.DataAnnotations;

namespace SportsVault.Entity
{
    public class Category
    {
        [Key]
        [Required]
        public int CategoryId { get; private set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category Name has to be between 2 and 100 characters.")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, MinimumLength = 2, ErrorMessage = "Description has to be between 2 and 500 characters.")]
        public string? Description { get; set; } = null!;
        
        [Required]
        public bool IsActive { get; set; } = true;         
        
        [Required]
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
