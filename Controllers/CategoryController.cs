using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsVault.Models;
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

        // optional helper method for CreatedAtAction
        [HttpGet("{name}")]
        [SwaggerOperation(Summary = "Get Category by Name", Description = "Fetches a single category by its name.")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            // If you don’t have a get-by-name yet, you can implement it later.
            return Ok();
        }

    }
}
