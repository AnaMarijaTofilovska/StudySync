using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Services
{
    public interface IUserService
    {
        // Pass through CRUD methods:
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> AddUserAsync(UserCreateDTO createDto);
        Task UpdateUserAsync(int id, UserUpdateDTO updateDto);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUserWithPendingTasksAsync();
       
        //add
        Task<bool> IsUserOverloadedAync(int userId, int taskThreshold);
        Task<IEnumerable<UserDTO>> GetAllTasksForUserAsync(int userId);
    }
}
