using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;
using StudySync.Services;

namespace StudySync.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>>GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody]CategoryCreateDTO categoryDto)
        {
            try
            {
               var createdCategory= await _categoryService.AddCategoryAsync(categoryDto);
                return CreatedAtAction(string.Empty, categoryDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>UpdateCategory(int id, CategoryUpdateDTO categoryDto)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryDto);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", ex.Message });
            }
        }

        [HttpGet("min-taks/{minTaskCount}")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>>CategoriesWithMinTasks(int minTaskCount)
        {
            var categories = await _categoryService.GetCategoriesWithMinTasksAsync(minTaskCount);
            return Ok(categories);
        }

        // GET: api/categories/completion-rate/{categoryId}
        [HttpGet("completion-rate/{categoryId}")]
        public async Task<IActionResult> GetCategoryTaskCompletionRate(int categoryId)
        {
            double completionRate = await _categoryService.GetCategoryTaskCompletionRateAsync(categoryId);
            return Ok(new { categoryId, completionRate });
        }

        // POST: api/categories/move-tasks
        [HttpPost("move-tasks")]
        public async Task<IActionResult> MoveTasksToAnotherCategory([FromQuery] int fromCategoryId, [FromQuery] int toCategoryId)
        {
            await _categoryService.MoveTasksToAnotherCategoryAsync(fromCategoryId, toCategoryId);
            return Ok(new { message = "Tasks moved successfully", fromCategoryId, toCategoryId });
        }
    }
}
