using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.NewFolder;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface ITaskRepository
    {
        //Task<List<UserTask>> GetAllTasks();
        Task<PagedResult<UserTask>> GetAllTasks(
      int? managerId, int? userId, DateTime? createdDate, TaskStatuses? status,
      string sortBy = "createdDate", bool isDescending = false, int page = 1, int pageSize = 10);


        Task<UserTask> GetTask(int taskId);
        Task<UserTask> CreateTask(UserTask task);
        Task<UserTask> UpdateTask(UserTask task);
        Task<UserTask> DeleteTask(UserTask task);


    }
}
