using SportsVault.Models;

namespace SportsVault.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);       
    }
}
