using StudySync.Models;

namespace StudySync.Repositories
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAllRemindersAsync();
        Task<Reminder?> GetReminderByIdAsync(int id);
        Task AddReminderAsync(Reminder reminder);
        Task UpdateReminderAsync(Reminder reminder);
        Task DeleteReminderAsync(int id);

        //This method should return reminders that are due within the next 24 hours.
        Task<IEnumerable<Reminder>> GetUpcomingRemindersAsync();
       
    }
}
