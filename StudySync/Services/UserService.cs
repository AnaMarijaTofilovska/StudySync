using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;

namespace StudySync.Services
{
    public class UserService : IUserService
    {
        private readonly IUserReposiotry _userRepository;
        private readonly ITaskItemRepository _taskRepository;
        private readonly IMapper _mapper;
        public UserService(IUserReposiotry userRepository,ITaskItemRepository taskRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _taskRepository = taskRepository;
            
        }

        // THE BASIC PASS-THORUGH CRUD OPERATIONS
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users= await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);    
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user= await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AddUserAsync(UserCreateDTO createDto)
        {
            //Validate user and email
            if (string.IsNullOrWhiteSpace(createDto.Name))
            {
                throw new ArgumentException("User name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(createDto.Email))
            {
                throw new ArgumentException("User email cannot be empty.");
            }
            
            var user = _mapper.Map<User>(createDto);
            await _userRepository.AddUserAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAsync(int id, UserUpdateDTO updateDto)
        {
            //Validate user and email
            if (string.IsNullOrWhiteSpace(updateDto.Name))
            {
                throw new ArgumentException("User name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Email))
            {
                throw new ArgumentException("User email cannot be empty.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if(user==null)
            {
                throw new KeyNotFoundException($"The user with {id} id is not found.");
            }

            _mapper.Map(updateDto, user);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetUserWithPendingTasksAsync()
        {
            var users= await _userRepository.GetUserWithPendingTasksAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }


        //NEW BUSSINESS LOGIC METHODS 

        public async Task<bool> IsUserOverloadedAync(int userId, int taskThreshold)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"The user with id {userId} not found.");
            }
            return user.Tasks.Count>taskThreshold; 
        }

        public async Task<IEnumerable<UserDTO>> GetAllTasksForUserAsync(int userId)
        {
            var allTasks = await _taskRepository.GetAllTasksAsync();
            var tasks = allTasks.Where(t => t.UserId == userId);
            return _mapper.Map<IEnumerable<UserDTO>>(tasks);
        }
    }
}
