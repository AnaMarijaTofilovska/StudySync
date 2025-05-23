using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _context;

        public ReminderRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<Reminder>> GetAllRemindersAsync()
        {
            return await _context.Reminders.ToListAsync();
        }

        public async Task<Reminder?> GetReminderByIdAsync(int id)
        {
            var reminder= await _context.Reminders.FindAsync(id);
            if(reminder == null)
            {
                throw new KeyNotFoundException($"The reminder with id {id} not found.");
            }
            return reminder;
        }

        public async Task AddReminderAsync(Reminder reminder)
        {
            await _context.Reminders.AddAsync(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReminderAsync(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReminderAsync(int id)
        {
            var reminder = await GetReminderByIdAsync(id);
            if( reminder != null)
            {
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reminder>> GetUpcomingRemindersAsync()
        {
            var now= DateTime.Now;
            var next24Hours = now.AddHours(24);

            return await _context.Reminders
                .Where(r => r.ReminderDateTime >= now && r.ReminderDateTime <= next24Hours)
                .ToListAsync();
        }

        


    }
}
