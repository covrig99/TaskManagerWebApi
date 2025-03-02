using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<UserTask>> GetAllTasks();
        Task<List<UserTask>> GetAllTasks(int userId);
       
        Task<UserTask> GetTask(int taskId);
        Task<UserTask> CreateTask(UserTask task);
        Task<UserTask> UpdateTask(UserTask task);
        Task<UserTask> DeleteTask(UserTask task);


    }
}
