using Microsoft.AspNetCore.Identity;
using System.Linq;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> CreateAccount(User users);
        
        Task<User> UpdateAccount(User users);
        Task<User> DeleteAccount(User users);
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(int? userId);
        Task<List<User>> GetUsers();
        Task<User> FindByEmail(string email);
        IQueryable<User> GetAllUsersQueryable();


    }
}
