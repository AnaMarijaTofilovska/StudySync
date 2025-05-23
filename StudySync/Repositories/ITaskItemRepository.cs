using StudySync.Models;

namespace StudySync.Repositories
{
    public interface ITaskItemRepository
    {
        //BASIC CRUD OPERATIONS (PASS THROUGH REPOSITORY)

        //Task is like reserved word, we call it asynchr.
        //Async is suffict, don't block the db keep it async.
        //return type is Task(becuase lets not wait ) is Task<>,
        //name of method in yellow
        //if is it list IEnurable if it looks like our table Taskitem

        Task<Taskitem> GetTaskByIdAsync(int id);
        Task<IEnumerable<Taskitem>> GetAllTasksAsync();
        Task AddTaskAsync(Taskitem task);
        Task UpdateTaskAsync(Taskitem task);
        Task DeleteTaskAsync(int id);

        //Add LINQ QUERY Definition then implement it 
        Task<IEnumerable<Taskitem>> GetIncompleteTaskAsync();
       



    }
}
