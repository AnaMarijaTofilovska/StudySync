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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO userDto)
        {
            try
            {
                var createdUser = await _userService.AddUserAsync(userDto);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id, createdUser });
            }
            catch(ArgumentException ex)
            {
               return BadRequest(ex.Message);   
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDto)
        {
            try
            {
                await _userService.UpdateUserAsync(id,userDto);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", ex.Message });
            }
        }

        [HttpGet("pending-tasks")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersWithPendingTasks()
        {
            var users = await _userService.GetUserWithPendingTasksAsync();
            return Ok(users);
        }


        // FOR THE NEW BUSSINESS LOGIC METHODS:

        //GET /api/users/1/isOverloaded?threshold=5:  This allows you to dynamically set the threshold in API requests! 
        [HttpGet("{id}/isOverloaded")]
        public async Task<IActionResult> IsUserOverloaded(int id, [FromQuery] int threshold)
        {
            var isOverloaded = await _userService.IsUserOverloadedAync(id, threshold);
            return Ok(new { userId = id, overloaded = isOverloaded });
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllTasksForUser(int id)
        {
            try
            {
                var user = await _userService.GetAllTasksForUserAsync(id);
                return Ok(user);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new {ex.Message});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occured", ex.Message });
            }
        }



    }
}
