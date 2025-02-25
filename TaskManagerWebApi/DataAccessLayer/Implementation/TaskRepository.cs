using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Implementation
{
    public class TaskRepository : ITaskRepository

    {
        private readonly DataContext context;
        public TaskRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Tasks> CreateTask(Tasks task)
        {
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> DeleteTask(Tasks task)
        {
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return task;
        }

        public async Task<List<Tasks>> GetAllTasks()
        {
            return await context.Tasks.OrderBy(c => c.UserId).ToListAsync();
        }

        public async Task<List<Tasks>> GetAllTasks(int userId)
        {
            return await context.Tasks.Where(s => s.UserId == userId).ToListAsync();
        }

        public async Task<List<Tasks>> GetAllTasks(string username)
        {
            var _user = context.Users.ToList().SingleOrDefault(s => s.Username == username);
            if (_user == null)
                return await context.Tasks.ToListAsync();
                
            return await context.Tasks.Where(x => x.UserId == _user.UserId).ToListAsync();
        }

        public async Task<Tasks> GetTask(int taskId)
        {
            return await context.Tasks.FindAsync(taskId);
        }

        public async Task<Tasks> UpdateTask(Tasks task)
        {
            context.Tasks.Update(task);
            await context.SaveChangesAsync();
            return task;
        }
    }
}
