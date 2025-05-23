using StudySync.Models;
using StudySync.Repositories;

namespace StudySync.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ITaskItemRepository _taskItemRepository;
        public ReminderService(IReminderRepository reminderRepository,TaskItemRepository taskItemRepository)
        {
            _reminderRepository = reminderRepository;
            _taskItemRepository = taskItemRepository;

        }
        //Basic pass-though crud operations

        public async Task<IEnumerable<Reminder>> GetAllRemindersAsync()
        {
            return await _reminderRepository.GetAllRemindersAsync();
        }

        public async Task<Reminder?> GetReminderByIdAsync(int id)
        {
            return await _reminderRepository.GetReminderByIdAsync(id);
        }

        public async Task AddReminderAsync(Reminder reminder)
        {
            // Validation: Ensure reminder date is in the future
            if (reminder.ReminderDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Reminder date must be in the future.");
            }
            await _reminderRepository.AddReminderAsync(reminder);
        }

        public async Task UpdateReminderAsync(Reminder reminder)
        {
            // Validation: Ensure reminder date is in the future
            if (reminder.ReminderDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Reminder date must be in the future.");
            }
            await _reminderRepository.UpdateReminderAsync(reminder);
        }

        public async Task DeleteReminderAsync(int id)
        {
            await _reminderRepository.DeleteReminderAsync(id);
        }

        public async Task<IEnumerable<Reminder>> GetUpcomingRemindersAsync()
        {
            return await _reminderRepository.GetUpcomingRemindersAsync();
        }

        //additional IDK IF RIGHTLY IMPLEMENTED : first in repo than in sevice 
        public async Task<IEnumerable<Reminder>> GetRemindersByTaskIdAsync(int taskId)
        {

            var task = await _taskItemRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with id {taskId} not found.");
            }

            var allReminders = await _reminderRepository.GetAllRemindersAsync();
            return allReminders.Where(r => r.TaskId == taskId).ToList();
        }
    }
}
