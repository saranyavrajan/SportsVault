using Microsoft.EntityFrameworkCore;
using SportsVault.Data;
using SportsVault.Models;
using SportsVault.Entity;
using SportsVault.Services.Interfaces;

namespace SportsVault.Services.Implementations
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        public async Task<CategoryDto> CreateCategoryAsync (CategoryDto categoryDto)
        {
            var entity = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description
            };

            context.Category.Add(entity);
            await context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryName = entity.CategoryName,
                Description = entity.Description
            };
        }
    }
}
