using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Tasks>> GetAllTasks();
        Task<List<Tasks>> GetAllTasks(int userId);
        Task<List<Tasks>> GetAllTasks(string username);
        Task<Tasks> GetTask(int taskId);
        Task<Tasks> CreateTask(Tasks task);
        Task<Tasks> UpdateTask(Tasks task);
        Task<Tasks> DeleteTask(Tasks task);


    }
}
