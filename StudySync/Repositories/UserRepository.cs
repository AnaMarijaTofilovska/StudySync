using Microsoft.EntityFrameworkCore;
using StudySync.Data;
using StudySync.Models;
using System.Threading.Tasks;

namespace StudySync.Repositories
{
    public class UserRepository : IUserReposiotry
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException($"User with id {id} not found");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user); //Tasks is the name of our table which we specified in AppDBContext 
            await _context.SaveChangesAsync(); // we need to push to db
        }



        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }

       
        public async Task<IEnumerable<User>> GetUserWithPendingTasksAsync()
        {
            //LINQ Query using method syntax

            return await _context.Users
                .Include(u => u.Tasks)
                .Where(u => u.Tasks.Any(t => !t.isCompleted))
                .ToListAsync();
        }
    }
}
