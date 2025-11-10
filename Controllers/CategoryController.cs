using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsVault.Models;
using SportsVault.Services.Implementations;
using SportsVault.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SportsVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        // Create a New Category
        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Category", Description = "Adds a new category with name and description. Returns the created category details.")]
        public async Task<IActionResult> CreateCategory (CategoryDto categoryDto)
        {
            var created = await categoryService.CreateCategoryAsync(categoryDto);

            if (created == null)
                return StatusCode(500, "An error occurred while creating the category.");

            return CreatedAtAction(nameof(GetCategoryByName), new { name = created.CategoryName }, created);
        }

        // Get Category details from Category Name
        [HttpGet("{name}")]
        [SwaggerOperation(Summary = "Get Category by Name", Description = "Fetches a single category by its name.")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {       
            var category = await categoryService.GetCategoryByNameAsync(name);  
            if (category == null)
                return NotFound($"Category with name '{name}' not found.");
            return Ok(category);
   
        }
    }
}
