using StudySync.Models;

namespace StudySync.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<Reminder>> GetAllRemindersAsync();
        Task<Reminder?> GetReminderByIdAsync(int id);
        Task AddReminderAsync(Reminder reminder);
        Task UpdateReminderAsync(Reminder reminder);
        Task DeleteReminderAsync(int id);
        Task<IEnumerable<Reminder>> GetUpcomingRemindersAsync();

        //additional
        Task<IEnumerable<Reminder>> GetRemindersByTaskIdAsync(int taskId);
    }
}
