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

        public Task<Tasks> CreateTask(Tasks task)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> DeleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetAllTasks(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetAllTasks(string username)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> GetTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> UpdateTask(Tasks task)
        {
            throw new NotImplementedException();
        }
    }
}
