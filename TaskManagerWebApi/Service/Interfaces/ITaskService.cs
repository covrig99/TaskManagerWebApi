using FluentResults;
using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.NewFolder;

namespace TaskManagerWebApi.Service.Interfaces
{
    public interface ITaskService
    {
        //public Task<List<UserTask>> GetAllTasks();
        public Task<Result<UserTask>> CreateTask(UserTask addedTask,User userLoginInfo);
        public Task<Result<UserTask>> UpdateTask(UserTask updateTask);
        public Task<Result<UserTask>> DeleteTask(int task);

        Task<PagedResult<UserTask>> GetAllTasks(TaskGetAllRequest request);


        public Task<Result<UserTask>> UpdateTaskStatus(int taskId, TaskStatuses newStatus, string? rejectionReason);

        public Task<Result<UserTask>> AssignTaskToUser(int taskId, int userId);

    }
}
