using StudySync.Models;

namespace StudySync.Repositories
{
    public interface ISubtaskRepository
    {
        Task<IEnumerable<Subtask>> GetAllSubtasksAsync();
        Task<Subtask?> GetSubtaskByIdAsync(int id);
        Task<IEnumerable<Subtask>> GetSubtasksbyTaskIdAsync(int taskId);
        Task AddSubtaskAsync(Subtask subtask);
        Task UpdateSubtaskAsync(Subtask subtask);
        Task DeleteSubtaskAsync(int id);

        //This method should return subtasks for a given TaskItem that are not marked as complete.
        Task<IEnumerable<Subtask>> GetIncompleteSubtasksByTaskIdAsync(int taskId);
    }
}
