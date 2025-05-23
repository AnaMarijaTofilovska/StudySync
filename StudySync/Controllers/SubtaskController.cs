using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;
using StudySync.Services;
using System.Threading.Tasks;

namespace StudySync.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubtaskController : ControllerBase
    {
        private readonly ISubtaskService _subtaskService;
        public SubtaskController(ISubtaskService subtaskService)
        {
            _subtaskService = subtaskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubtaskDTO>>> GetAllSubtasksAsync()
        {
            var subtasks = await _subtaskService.GetAllSubtasksAsync();
            return Ok(subtasks);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<SubtaskDTO>>> GetSubtasksbyTaskIdAsync(int taskId)
        {
            var subtasks = await _subtaskService.GetSubtasksbyTaskIdAsync(taskId);
            return Ok(subtasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubtaskDTO>> GetSubtaskByIdAsync(int id)
        {
            var subtask = await _subtaskService.GetSubtaskByIdAsync(id);
            return Ok(subtask);
        }

        [HttpPost]
        public async Task<ActionResult<SubtaskDTO>> AddSubtaskAsync(SubtaskCreateDTO categoryDto)
        {
            try
            {
                await _subtaskService.AddSubtaskAsync(categoryDto);
                return CreatedAtAction(string.Empty, categoryDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubtaskAsync(int id, SubtaskUpdateDTO categoryDto)
        {
            try
            {
                await _subtaskService.UpdateSubtaskAsync(id, categoryDto);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message="An error ocurred", ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubtaskAsync(int id)
        {
            try
            {
                await _subtaskService.DeleteSubtaskAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error ocurred", ex.Message });
            }
        }

        [HttpGet("incompelte/{taskId}")]
        public async Task<ActionResult<IEnumerable<SubtaskDTO>>> GetIncompleteSubtasksByTaskIdAsync(int taskId)
        {
            try
            {
                var subtasks = await _subtaskService.GetIncompleteSubtasksByTaskIdAsync(taskId);
                return Ok(subtasks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error ocurred", ex.Message });
            }
        }
    }
}
