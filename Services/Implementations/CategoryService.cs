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

        public async Task<CategoryDto?> GetCategoryByNameAsync(string name)
        {
            var entity = await context.Category
                .FirstOrDefaultAsync(c => c.CategoryName == name);

            if (entity == null)
                return null;

            return new CategoryDto
            {
                CategoryName = entity.CategoryName,
                Description = entity.Description
            };
        }
    }
}
