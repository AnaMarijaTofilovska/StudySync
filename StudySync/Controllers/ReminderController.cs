using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySync.Models;
using StudySync.Repositories;
using StudySync.Services;
using System.Threading.Tasks;

namespace StudySync.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }


        [HttpGet]
        public async Task<ActionResult<Reminder>> GetReminders()
        {
            var reminders = await _reminderService.GetAllRemindersAsync();
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetReminder(int id)
        {
            var reminder = await _reminderService.GetReminderByIdAsync(id);
            if(reminder == null)
            {
                return NotFound();
            }
            return Ok(reminder);
        }

        [HttpPost]
        public async Task<ActionResult<Reminder>> CreateReminder([FromBody] Reminder reminder)
        {
            await _reminderService.AddReminderAsync(reminder);
            return Created(string.Empty, reminder);
        }

        [HttpGet("upcoming-reminders")]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetUpcomingReminders()
        {
            var reminders = await _reminderService.GetUpcomingRemindersAsync();
            return Ok(reminders);
        }

        //Additional
        // Get all reminders by task ID
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetRemindersByTaskId(int taskId)
        {
            var reminders = await _reminderService.GetRemindersByTaskIdAsync(taskId);
            if (reminders == null || !reminders.Any())
            {
                return NotFound("No reminders found for the given task.");
            }
            return Ok(reminders);
        }
    }
}
