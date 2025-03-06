using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DataAccessLayer.Implementation;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.Errors;
using TaskManagerWebApi.Service.Interfaces;

namespace TaskManagerWebApi.Service.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;
        public TaskService(ITaskRepository taskRepository, IAccountRepository accountRepository, UserManager<User> userManager)

        {
            _taskRepository = taskRepository;
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        public async Task<Result<UserTask>> AssignTaskToUser(int taskId, int userId)
        {

            var task = await _taskRepository.GetTask(taskId);
            if (task == null) throw new Exception("Task not found");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.Role != "User") throw new Exception("Invalid user");

            task.IdUser = userId;
            await _taskRepository.UpdateTask(task);
            return Result.Ok(task);
        }

        public async Task<Result<UserTask>> CreateTask(UserTask addedTask, User userLoginInfo)
        {
            var userExists = await _accountRepository.GetUser(addedTask.IdUser);
            if (userExists == null)
            {
                return Result.Fail("User not found. Cannot assign task.");
            }

            addedTask.User = userExists; 
            addedTask.IdUser = userExists.Id; 
            addedTask.CreatedDate = DateTime.UtcNow; 
            addedTask.UpdatedDate = DateTime.UtcNow;

            await _taskRepository.CreateTask(addedTask);
            return Result.Ok(addedTask);
        }

        public async Task<Result<UserTask>> DeleteTask(int task)
        {
            var addedTask = await _taskRepository.GetTask(task);
            await _taskRepository.DeleteTask(addedTask);
                return addedTask;
        }

        public async Task<List<UserTask>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            foreach (var task in tasks)
            {
                task.RejectionReason ??= "N/A";
                task.User ??= new User { Id = 0, Email = "Unassigned", UserName = "No User" };
            }
            return tasks;
        }

        public async Task<Result<UserTask>> UpdateTask(UserTask updateTask)
        {
            var taskfound = await _taskRepository.GetTask(updateTask.Id);
            if(taskfound == null)
            {
                return Result.Fail(ApiErrors.TaskNotAvailable);
            }
            taskfound.Title = updateTask.Title;
            taskfound.Description = updateTask.Description;
            taskfound.Status = updateTask.Status;
            taskfound.RejectionReason = updateTask.RejectionReason;

            if (updateTask.IdUser > 0) 
            {
                var user = await _accountRepository.GetUser(updateTask.IdUser);
                if (user == null)
                {
                    return Result.Fail(ApiErrors.UserNotFound);
                }
                taskfound.IdUser = updateTask.IdUser; // Assign the correct UserId
                taskfound.User = user; // Ensure navigation property is populated
            }

            await _taskRepository.UpdateTask(taskfound);
            return Result.Ok(taskfound);
        }
    }
}
