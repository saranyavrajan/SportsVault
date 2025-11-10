using SportsVault.Models;

namespace SportsVault.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
        Task<CategoryDto?> GetCategoryByNameAsync(string name);
    }
}
