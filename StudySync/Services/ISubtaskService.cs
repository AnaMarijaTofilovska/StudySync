using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Services
{
    public interface ISubtaskService
    {

        Task<IEnumerable<SubtaskDTO>> GetAllSubtasksAsync();
        Task<IEnumerable<SubtaskDTO>> GetSubtasksbyTaskIdAsync(int taskId);
        Task<SubtaskDTO> GetSubtaskByIdAsync(int id);
        Task<SubtaskDTO> AddSubtaskAsync(SubtaskCreateDTO createDto);
        Task UpdateSubtaskAsync(int id, SubtaskUpdateDTO updateDto);
        Task DeleteSubtaskAsync(int id);

        //This method should return subtasks for a given TaskItem that are not marked as complete.
        Task<IEnumerable<SubtaskDTO>> GetIncompleteSubtasksByTaskIdAsync(int taskId);
    }
}
