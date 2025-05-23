using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;

namespace StudySync.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        //we have to inherit the interface, click on error icon and implement interface 
        //to be able to access db connection we need the session ,to be injected define variable which will keep session localy and inject it, using context
        //INJECT YOUR DB SESSION
        private readonly ApplicationDbContext _context;
        public TaskItemRepository(ApplicationDbContext context)
        { 
            _context = context;
        }

        //READ ALL ITEMS
        public async Task<IEnumerable<Taskitem>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        //READ ONE ITEM
        public async Task<Taskitem> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task is null)
            {
                throw new KeyNotFoundException($"Task with id {id} not found");
            }
            return task;
        }


        //CREATE
        public async Task AddTaskAsync(Taskitem task)
        {
            await _context.Tasks.AddAsync(task); //Tasks is the name of our table which we specified in AppDBContext 
            await _context.SaveChangesAsync(); // we need to push to db
        }


        //UPDATE
        public async Task UpdateTaskAsync(Taskitem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

        }


        //DELETE
        public async Task DeleteTaskAsync(int id)
        {
            var task=await GetTaskByIdAsync(id);
            if(task !=null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

       
        public async Task<IEnumerable<Taskitem>> GetIncompleteTaskAsync()
        {
            //LINQ Query using method syntax
            return await _context.Tasks
                .Where(t=> !t.isCompleted)
                .OrderBy(t=>t.DueDate)
                .ToListAsync();
        }



       
    }
}
