using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Services
{
    public interface ITaskItemService
    {
        //The Interfaces are passed and used in other layers, that's why we create them 
        //Same as Repository Interface 

        //Now whenever we had Taskitem we have to have TaskitemDTo, dont bring all the data bring only taskdto , when you do create 
        // RE-UPDATE THESE TO RETURN <TaskItemDTO> instead of <TaskItem>

        Task<TaskItemDTO> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItemDTO>> GetAllTasksAsync();
        Task<TaskItemDTO> AddTaskAsync(TaskItemCreateDTO task);
        Task UpdateTaskAsync(int id, TaskItemUpdateDTO task);
        Task DeleteTaskAsync(int id);
      

        Task<IEnumerable<Taskitem>> GetIncompleteTaskAsync();

        //Bussiness logic method 
        Task<bool> ToggleTaskCompletitionAsync(int id);
        Task<IEnumerable<TaskItemDTO>> GetTaksByPriorityAsync(string priority);
        Task<IEnumerable<TaskItemDTO>> GetTaskByCategoryAsync(int categoryId);
        Task<IEnumerable<TaskItemDTO>> GetUpcomingTasksAsync(int days);
        Task AssignTaskToUserAsync(int taskId, int userId);

    }
}
