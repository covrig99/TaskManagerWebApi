using Microsoft.EntityFrameworkCore;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext context;
        public AccountRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Users> CreateAccount(Users user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> DeleteAccount(Users user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<List<Users>> GetAllUsers(Users users)
        {
            return await context.Users.OrderBy(c => c.UserId).ToListAsync();
        }

        public async Task<Users> GetUser(int userId)
        {
            return await context.Users.FindAsync(userId);
        }

        public async Task<Users> UpdateAccount(Users user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;

        }

        public bool VerifyAccount(Users users)
        {
            return context.Users.Any(x => x.Username == users.Username && x.Password == users.Password);
        }
    }
}
