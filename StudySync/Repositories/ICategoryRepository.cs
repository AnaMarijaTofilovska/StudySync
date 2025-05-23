using StudySync.Models;

namespace StudySync.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);

        //This method should return categories where the count of related tasks is greater than or equal to minTaskCount.
        Task<IEnumerable<Category>> GetCategoriesWithMinTasksAsync(int minTaskCount);
    }
}
