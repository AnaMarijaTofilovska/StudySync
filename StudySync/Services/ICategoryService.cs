using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> AddCategoryAsync(CategoryCreateDTO createDto);
        Task UpdateCategoryAsync(int id,CategoryUpdateDTO updateDto);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetCategoriesWithMinTasksAsync(int minTaskCount);


        //Additional
        Task<double> GetCategoryTaskCompletionRateAsync(int categoryId);
        Task MoveTasksToAnotherCategoryAsync(int fromCategoryId, int toCategoryId);
    }
}
