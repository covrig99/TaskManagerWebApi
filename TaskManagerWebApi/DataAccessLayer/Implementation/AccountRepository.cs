using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TaskManageDbContext context;
        
        public AccountRepository(TaskManageDbContext context)
        {
            this.context = context;
            
        }

        public async Task<User> CreateAccount(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteAccount(User user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> FindByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await context.Users.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<User> GetUser(int? userId)
        {
            return await context.Users.FindAsync(userId);
        }
        
        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateAccount(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;

        }

       
    }
}
