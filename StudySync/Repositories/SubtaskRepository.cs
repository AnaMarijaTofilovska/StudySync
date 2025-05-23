using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Repositories
{
    public class SubtaskRepository : ISubtaskRepository
    {
        private readonly ApplicationDbContext _context;
        public SubtaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subtask>> GetAllSubtasksAsync()
        {
            return await _context.Subtasks.ToListAsync();
        }

        public async Task<Subtask>GetSubtaskByIdAsync(int id)
        {
            var subtask = await _context.Subtasks.FindAsync(id);
            if (subtask == null)
            {
                throw new KeyNotFoundException($"Subtask with id {id} not found.");
            }
            return subtask;
        }

        public async Task<IEnumerable<Subtask>> GetSubtasksbyTaskIdAsync(int taskId)
        {
            return await _context.Subtasks
            .Where(s => s.TaskId == taskId)
            .ToListAsync();
        }

        public async Task AddSubtaskAsync(Subtask subtask)
        {
            await _context.Subtasks.AddAsync(subtask);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubtaskAsync(Subtask subtask)
        {
            _context.Subtasks.Update(subtask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubtaskAsync(int id)
        {
            var subtask = await _context.Subtasks.FindAsync(id);
            if (subtask != null)
            {
                _context.Subtasks.Remove(subtask);
                await _context.SaveChangesAsync();
            }

        }

        // Return subtasks for a given taskItem that are not marked as complete
        public async Task<IEnumerable<Subtask>> GetIncompleteSubtasksByTaskIdAsync(int taskId)
        {
            return await _context.Subtasks
               .Where(s => s.TaskId == taskId && !s.isCompleted)
               .ToListAsync();
        }

       

       
    }
}
