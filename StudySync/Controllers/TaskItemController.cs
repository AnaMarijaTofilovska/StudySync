using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;
using StudySync.Services;

namespace StudySync.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        //1. for our controller we need repository , the repository is conn to db 
        private readonly ITaskItemService _taskService;

        public TaskItemController(ITaskItemService taskService)
        {
            _taskService = taskService;
        }

        //ENDPONTS: PUT,POST,GET,DELETE
        // method get which will show to user



        //We update using DTO
        //GET TASK BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDTO>> GetTaskById(int id)
        {
            //try to find my task
            var task = await _taskService.GetTaskByIdAsync(id);

            //if not found
           if(task==null)
            {
                //say no found
                return NotFound();
            }
           //else return taask
            return Ok(task);
        }

        // GET ALL TASKS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTasks()
        {
            var task = await _taskService.GetAllTasksAsync();
            return Ok(task); //after you get data say okay got the data
        }

        //For now we will just return an empty string
        // create a new task
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TaskItemDTO>> CreateTask([FromBody] TaskItemCreateDTO taskDto)
        {
            try
            {
                var createdTask = await _taskService.AddTaskAsync(taskDto);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskItemUpdateDTO taskItemUpdateDTO)
        {
            try
            {
                await _taskService.UpdateTaskAsync(id, taskItemUpdateDTO);
                return NoContent();  // Successful update, no content to return
            }
            catch (KeyNotFoundException)
            {
                return NotFound();  // Task not found
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();  // HTTP 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });  // HTTP 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });  // HTTP 500 Internal Server Error
            }
        }


        //Expose endpoint for method to get incomplete tasks  
        [HttpGet("incomplete-tasks")]
        public async Task<ActionResult<IEnumerable<Taskitem>>> GetIncompleteTask()
        {

            var incomplete = await _taskService.GetIncompleteTaskAsync();
            return Ok(incomplete);

        }

        //New Bussiness Logic Implementations
        // New endpoint for toggling task completion
        [HttpPatch("{id}/toggle-completion")]
        public async Task<IActionResult> ToggleTaskCompletion(int id)
        {
            try
            {
                var result = await _taskService.ToggleTaskCompletitionAsync(id);
                return Ok(result);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", error = ex.Message });
            }
           
        }

        // GET: api/TaskItem/priority/High
        [HttpGet("priority")]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTasksByPriority([FromQuery(Name = "value")] string priority)
        {
            try
            {
                if (string.IsNullOrEmpty(priority))
                {
                    return BadRequest("Priority value is required");
                }

                var tasks = await _taskService.GetTaksByPriorityAsync(priority);
                return Ok(tasks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/TaskItem/upcoming/7
        [HttpGet("upcoming/{days}")]
        public async Task<ActionResult<IEnumerable<Taskitem>>> GetUpcomingTasks(int days)
        {
            try
            {
                var tasks = await _taskService.GetUpcomingTasksAsync(days);
                return Ok(tasks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/TaskItem/5/assign/2
        [HttpPost("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignTaskToUser(int taskId, int userId)
        {
            try
            {
                await _taskService.AssignTaskToUserAsync(taskId, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTasksByCategory(int categoryId)
        {
            try
            {
                var tasks = await _taskService.GetTaskByCategoryAsync(categoryId);
                return Ok(tasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", error = ex.Message });
            }
        }



    }
}
