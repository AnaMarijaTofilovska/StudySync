using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;
using System.Threading.Tasks;

namespace StudySync.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskRepository;
        private readonly IUserReposiotry _userRepository;
        private readonly IMapper _mapper;

        //Dependency Injection of INTERFACE REPOSITORY(FOR CRUD OPERATIONS)
        //GET DATA FROM REPOSITORY , SO CONTROLLER GETS FROM SERVICE

        public TaskItemService (ITaskItemRepository taskRepository, IUserReposiotry userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            //1.Inject automapper
            _mapper = mapper;
        }

        //Step 1: Basic pass through implementation
        public async Task<TaskItemDTO> GetTaskByIdAsync(int id)
        {
            var taskItem = await _taskRepository.GetTaskByIdAsync(id);

            return _mapper.Map<TaskItemDTO>(taskItem);
        }

        public async Task<IEnumerable<TaskItemDTO>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            return _mapper.Map<IEnumerable<TaskItemDTO>>(tasks);
        }

        //Step 2: They are anemic first,so we add logic for the service , so change add and update
        //As a user i should be able to add task to system,but not date in the past, and assign one priority

        public async Task<TaskItemDTO> AddTaskAsync(TaskItemCreateDTO createDto)
        {

            //Validation due date
            if (createDto.DueDate < DateTime.Now)
            {
                throw new ArgumentException("Due date must be in the future.");
            }
            
            //Validate priority
            if(createDto.Priority!="Low" && createDto.Priority != "Medium" && createDto.Priority != "High")
            {
                throw new ArgumentException("Priority must be 'Low','Medium' or 'High'");
            }

            // Map input DTO to domain entity
            var taskItem = _mapper.Map<Taskitem>(createDto);

            // Save to repository
            await _taskRepository.AddTaskAsync(taskItem);

            // Map back to DTO and return
            return _mapper.Map<TaskItemDTO>(taskItem);


        }

        public async Task UpdateTaskAsync(int id,TaskItemUpdateDTO updateDto)
        {
            if (string.IsNullOrWhiteSpace(updateDto.Title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            //We dont need due date validation

            //Validate priority
            if (updateDto.Priority != "Low" && updateDto.Priority != "Medium" && updateDto.Priority != "High")
            {
                throw new ArgumentException("Priority must be 'Low','Medium' or 'High'");
            }

            // Get existing task by id
            var existingTask = await _taskRepository.GetTaskByIdAsync(id); // Use `id` here to match the method signature
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            // Map the updated values from the DTO to the existing task
            _mapper.Map(updateDto, existingTask);

            // Update the task in the repository
            await _taskRepository.UpdateTaskAsync(existingTask);

        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }

        public async Task<IEnumerable<Taskitem>> GetIncompleteTaskAsync()
        {
            return await _taskRepository.GetIncompleteTaskAsync();
        }


        //Step 3: Implement bussiness logic for toggling task completition
        //Bussiness Logic: Go get item with that id, and change the isCompleted , and update data in repo,the repo in db
        public async Task<bool> ToggleTaskCompletitionAsync(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return false;
                }


                //Toggle the completition 
                task.isCompleted = !task.isCompleted;

                //Update the task 
                await _taskRepository.UpdateTaskAsync(task);

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        public async Task<IEnumerable<TaskItemDTO>> GetTaksByPriorityAsync(string priority)
        {

            //Validate priority

            if (priority != "Low" && priority != "Medium" && priority != "High")
            {
                throw new ArgumentException("Priority must be 'Low', 'Medium', or 'High'.");
            }

            var allTasks= await _taskRepository.GetAllTasksAsync();
            var filteredTasks = allTasks.Where(t => t.Priority == priority);
            return _mapper.Map<IEnumerable<TaskItemDTO>>(filteredTasks);
        }

        public async Task<IEnumerable<TaskItemDTO>> GetUpcomingTasksAsync(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Days must be greater than zero.");
            }

            // Calculate the date range
            var endDate = DateTime.Now.AddDays(days);

            // Get all tasks and filter by due date
            var allTasks = await _taskRepository.GetAllTasksAsync();
            var upcomingTasks = allTasks
             .Where(t => t.DueDate >= DateTime.Now && t.DueDate <= endDate)
             .OrderBy(t => t.DueDate);

            return _mapper.Map<IEnumerable<TaskItemDTO>>(upcomingTasks);
        }

        public async Task AssignTaskToUserAsync(int taskId, int userId)
        {
            // Verify task exists
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskId} not found.");
            }

            // Verify user exists
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Update the task's UserId
            task.UserId = userId;

            // Save the changes
            await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task<IEnumerable<TaskItemDTO>> GetTaskByCategoryAsync(int categoryId)
        {
            var allTasks = await _taskRepository.GetAllTasksAsync();
            var filteredTasks = allTasks.Where(t => t.CategoryId == categoryId);
            return _mapper.Map<IEnumerable<TaskItemDTO>>(filteredTasks);
        }
    }
}
