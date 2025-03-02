using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<string> Login(UserLoginInfo userLoginInfo);
        Task<User> CreateAccount(User users);
        
        Task<User> UpdateAccount(User users);
        Task<User> DeleteAccount(int user);
        Task<List<User>> GetAllUsers(User users);
        Task<User> GetUser(int userId);
        
        public Task<UserLoginInfo> UpdatePassword(UserLoginInfo userLoginInfo, string newPassword);
    }
}
