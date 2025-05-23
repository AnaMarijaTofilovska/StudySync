using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;

namespace StudySync.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITaskItemRepository _taskRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ITaskItemRepository taskRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        //The pass throygh CRUD methods

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories= await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryCreateDTO createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            await _categoryRepository.AddCategoryAsync(category);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryUpdateDTO updateDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if(category == null)
            {
                throw new KeyNotFoundException($"The category with id {id} not found");
            }

            _mapper.Map(category, updateDto);
            await _categoryRepository.UpdateCategoryAsync(category);
            
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesWithMinTasksAsync(int minTaskCount)
        {
            var categories= await _categoryRepository.GetCategoriesWithMinTasksAsync(minTaskCount);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }


        //ADDITIONAL

        public async Task<double> GetCategoryTaskCompletionRateAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null || category.Tasks.Count == 0)
            {
                return 0.0; // No tasks = 0% completition rate
            }

            int completedTasks = category.Tasks.Count(t => t.isCompleted);
            double completitionRate = (double)completedTasks / category.Tasks.Count * 100;
            return completitionRate;
        }

        public async Task MoveTasksToAnotherCategoryAsync(int fromCategoryId, int toCategoryId)
        {
            var fromCategory = await _categoryRepository.GetCategoryByIdAsync(fromCategoryId);
            var toCategory = await _categoryRepository.GetCategoryByIdAsync(toCategoryId);

            //Validation 
            if (fromCategory == null || toCategory == null)
            {
                throw new KeyNotFoundException("One or both categories not found.");

            }

            foreach (var task in fromCategory.Tasks)
            {
                task.CategoryId = toCategoryId;
                await _taskRepository.UpdateTaskAsync(task);
            }
        }
    }
}
