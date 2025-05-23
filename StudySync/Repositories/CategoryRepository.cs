using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                throw new KeyNotFoundException($"The category with id {id} not found.");
            }
            return category;
        }


        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if(category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();  
            }
        }


        // Custom LINQ Query: Get categories with at least a specified number of tasks 
        public async Task<IEnumerable<Category>> GetCategoriesWithMinTasksAsync(int minTaskCount)
        {
            return await _context.Categories
                        .Include(c => c.Tasks)
                        .Where(c => c.Tasks.Count >= minTaskCount)
                        .ToListAsync();
        }

        
        
    }
}
