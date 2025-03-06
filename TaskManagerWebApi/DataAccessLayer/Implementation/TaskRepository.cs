using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

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

        public async Task<List<UserTask>> GetAllTasks()
        {
            return await context.Tasks
        .Include(t => t.User) 
        .ToListAsync();
        }

        public async Task<List<UserTask>> GetAllTasks(int userId)
        {
            return await context.Tasks.Where(s => s.IdUser == userId).ToListAsync();
        }


        public async Task<UserTask> GetTask(int taskId)
        {
            return await context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId);


        }

        public async Task<UserTask> UpdateTask(UserTask task)
        {
            context.Tasks.Update(task);
            await context.SaveChangesAsync();
            return task;
        }
    }
}
