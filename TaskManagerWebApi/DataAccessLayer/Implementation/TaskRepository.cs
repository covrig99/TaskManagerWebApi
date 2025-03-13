using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.NewFolder;

namespace TaskManagerWebApi.DataAccessLayer.Implementation
{
    public class TaskRepository : ITaskRepository

    {
        private readonly TaskManageDbContext context;
        public TaskRepository(TaskManageDbContext context)
        {
            this.context = context;
        }

        public async Task<UserTask> CreateTask(UserTask task)
        {
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask> DeleteTask(UserTask task)
        {
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return task;
        }

        public async Task<PagedResult<UserTask>> GetAllTasks(
      int? managerId, int? userId, DateTime? createdDate, TaskStatuses? status,
      string sortBy = "createdDate", bool isDescending = false, int page = 1, int pageSize = 10)
        {
            var query = context.Tasks.Include(t => t.User).AsQueryable();

            // Filtering
            if (managerId.HasValue)
                query = query.Where(t => t.User.Id == managerId.Value);

            if (userId.HasValue)
                query = query.Where(t => t.IdUser == userId.Value);

            if (createdDate.HasValue)
                query = query.Where(t => t.CreatedDate.Date == createdDate.Value.Date);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            // Sorting
            query = sortBy.ToLower() switch
            {
                "createddate" => isDescending ? query.OrderByDescending(t => t.CreatedDate) : query.OrderBy(t => t.CreatedDate),
                "status" => isDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
                _ => query.OrderByDescending(t => t.CreatedDate) // Default sorting by CreatedDate DESC
            };

            // Pagination
            int totalCount = await query.CountAsync();
            var tasks = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<UserTask>(tasks, totalCount, page, pageSize);
        }

        //public async Task<List<UserTask>> GetAllTasks(int userId)
        //{
        //    return await context.Tasks.Where(s => s.IdUser == userId).ToListAsync();
        //}


        public async Task<UserTask> GetTask(int taskId)
        {
            return await context.Tasks
                .FindAsync(taskId);


        }

        public async Task<UserTask> UpdateTask(UserTask task)
        {
            context.Tasks.Update(task);
            await context.SaveChangesAsync();
            return task;
        }
    }
}
