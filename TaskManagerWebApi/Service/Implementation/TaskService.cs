using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Implementation;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.Errors;
using TaskManagerWebApi.Models.NewFolder;
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
            task.UpdatedDate = DateTime.UtcNow;
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

        public async Task<PagedResult<UserTask>> GetAllTasks(TaskGetAllRequest request)
        {
            var tasksQuery = _taskRepository.GetAllTasksQueryable();

            if (request.ManagerId.HasValue)
                tasksQuery = tasksQuery.Where(t => t.User.Id == request.ManagerId);

            if (request.UserId.HasValue)
                tasksQuery = tasksQuery.Where(t => t.User.Id == request.UserId);

            if (request.CreatedDate.HasValue)
                tasksQuery = tasksQuery.Where(t => t.CreatedDate.Date == request.CreatedDate.Value.Date);

            if (request.Status.HasValue)
                tasksQuery = tasksQuery.Where(t => t.Status == request.Status);

            if (!string.IsNullOrEmpty(request.SortDirection))
            {
                bool isDescending = request.SortDirection == "DESC";
                tasksQuery = isDescending
                    ? tasksQuery.OrderByDescending(t => EF.Property<object>(t, request.SortBy))
                    : tasksQuery.OrderBy(t => EF.Property<object>(t, request.SortBy));
            }

            int totalTasks = await tasksQuery.CountAsync();
            var tasks = await tasksQuery
                .Skip(request.Offset ?? 0)
                .Take(request.Limit ?? 10)
                .ToListAsync();

            return new PagedResult<UserTask>(totalTasks, request.Offset ?? 0, request.Limit ?? 10, tasks);
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
                taskfound.IdUser = updateTask.IdUser; 
                taskfound.User = user; 
            }

            await _taskRepository.UpdateTask(taskfound);
            return Result.Ok(taskfound);
        }
        public async Task<Result<UserTask>> UpdateTaskStatus(int taskId, TaskStatuses newStatus, string? rejectionReason)
        {
            var task = await _taskRepository.GetTask(taskId);
            if (task == null)
                return Result.Fail(ApiErrors.TaskNotFound);



            if (task.Status == TaskStatuses.ToDo)
            {
                if (newStatus == TaskStatuses.Done || newStatus == TaskStatuses.Approved)
                    return Result.Fail(ApiErrors.InvalidStatusUpdateError);

                if (newStatus == TaskStatuses.InProgress)
                {
                    task.Status = newStatus;
                }
            }
            else if (newStatus == TaskStatuses.Rejected)
            {
                if (string.IsNullOrWhiteSpace(rejectionReason))
                    return Result.Fail(ApiErrors.EmptyMandatoryReason);

                task.Status = TaskStatuses.Rejected;
                task.RejectionReason = rejectionReason;
            }
            else
            {
                
                task.Status = newStatus;
            }

            task.UpdatedDate = DateTime.UtcNow;
            await _taskRepository.UpdateTask(task);

            return Result.Ok(task);
        }
    }
}
