using FluentResults;
using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.Service.Interfaces
{
    public interface ITaskService
    {
        public Task<List<UserTask>> GetAllTasks();
        public Task<Result<UserTask>> CreateTask(UserTask addedTask,User userLoginInfo);
        public Task<Result<UserTask>> UpdateTask(UserTask updateTask);
        public Task<Result<UserTask>> DeleteTask(int task);


       
        public Task<Result<UserTask>> UpdateTaskStatus(int taskId, TaskStatuses newStatus, string? rejectionReason);

        public Task<Result<UserTask>> AssignTaskToUser(int taskId, int userId);

    }
}
